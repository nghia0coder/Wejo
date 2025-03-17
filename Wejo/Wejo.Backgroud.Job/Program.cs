using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Background.Job;

using Common.Core.Extensions;
using Common.Domain.Database;
using Common.Domain.Interfaces;
using Common.SeedWork.Extensions;
using Interfaces;
using Services;
using static Common.SeedWork.Constants.Setting;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHealthChecks();

        var me = typeof(Program);
        var assembly = me.Assembly.GetName().Name;

        // Load settings from the environment
        var st = _prefix.ConvertEnvironmentVariable<Setting>(CommonPrefix);
        st.Prefix = _prefix;

        // Load connection string from appsettings.json
        var config = new ConfigurationBuilder().AddConfiguration(builder.Configuration).Build();
        var cs = config.GetConnectionString("DefaultConnection");

        // Update connection string
        var csDb = cs.SetDbParams(st.Db);

        // Setup Dependency Injection
        builder.Services.AddSingleton<ISetting>(st!);
        builder.Services.AddHangfire(config => config.UsePostgreSqlStorage(p => p.UseNpgsqlConnection(csDb!)));
        builder.Services.AddHangfireServer();
        builder.Services.AddDbContext<WejoContext>(p => p.UseNpgsql(csDb!, p => p.MigrationsAssembly(assembly).EnableRetryOnFailure()), ServiceLifetime.Scoped);
        builder.Services.AddScoped<IWejoContext>(p => p.GetService<WejoContext>()!);
        builder.Services.AddScoped<IGameService, GameService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(p => { p.EnableAnnotations(); });

        var app = builder.Build();

        // Fix for PostgreSQL timestamp behavior
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment() || st.SwaggerEnabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }

        // CORS setup
        var origins = st.Origins == null ? [] : st.Origins.Split(';');
        if (origins.Length > 0)
        {
            app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().WithOrigins(origins).AllowCredentials());
        }

        // Enable Hangfire Dashboard
        app.UseHangfireDashboard();

        // Schedule the recurring job
        RecurringJob.AddOrUpdate<IGameService>(
                recurringJobId: "UpdateGameStatusJob",
                methodCall: service => service.UpdateGameStatusAsync(),
                cronExpression: Cron.Minutely,
                options: new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Utc,
                }
        );

        app.Run();
    }

    private static string _prefix = "Job";
}