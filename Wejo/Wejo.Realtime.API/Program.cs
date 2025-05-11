using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Wejo.Realtime.API;

using Common.Core.Extensions;
using Common.Domain.Database;
using Common.Domain.Interfaces;
using Common.SeedWork.Extensions;
using Hubs;
using MessageQueue;
using Services;
using static Common.SeedWork.Constants.Setting;

/// <summary>
/// Program
/// </summary>
public class Program
{
    #region -- Methods --

    /// <summary>
    /// Main
    /// </summary>
    /// <param name="args">Arguments</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHealthChecks();

        #region -- Load configuration environment --
        var environment = builder.Environment.EnvironmentName;
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        var me = typeof(Program);
        var assembly = me.Assembly.GetName().Name;

        // Load settings from the environment
        var st = _prefix.ConvertEnvironmentVariable<Setting>(CommonPrefix);
        st.Prefix = _prefix;

        // Load connection string appsettings.json
        var config = new ConfigurationBuilder().AddConfiguration(builder.Configuration).Build();
        var cs = config.GetConnectionString("DefaultConnection");

        // Update connection string
        var csDb = cs.SetDbParams(st.Db);
        builder.Services.AddDbContext<WejoContext>(options =>
          options.UseNpgsql(csDb, o => o.UseNetTopologySuite())
        );
        #endregion

        #region -- Load Http Protocols --
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(8083, listenOptions =>
            {
                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
            });
            options.ListenAnyIP(5001, listenOptions =>
            {
                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                Console.WriteLine("Debug: Kestrel configured HTTP/2 cleartext on 5001 for gRPC");
            });
        });
        #endregion

        #region -- Setup DI --
        // Setting
        builder.Services.AddSingleton<ISetting>(st!);

        builder.Services.AddHostedService<GameChatConsumer>();

        builder.Services.AddHostedService<PlaypalChatConsumer>();

        // DbContext
        builder.Services.AddDbContext<WejoContext>(p => p.UseNpgsql(csDb!, p => p.MigrationsAssembly(assembly).EnableRetryOnFailure()), ServiceLifetime.Scoped);
        builder.Services.AddScoped<IWejoContext>(p => p.GetService<WejoContext>()!);

        // SignalR
        builder.Services.AddSignalR();

        // gRPC
        builder.Services.AddGrpc();

        #endregion

        #region -- Setup token --
        var firebaseProjectId = builder.Configuration["Firebase:ProjectId"];
        builder.Services.AddBearerAuthentication(firebaseProjectId);
        builder.Services.AddResponseCaching();

        // Cookie name
        builder.Services.ConfigureApplicationCookie(p => { p.Cookie.Name = _prefix; });
        #endregion

        #region -- Swagger and CORS --
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(p => { p.EnableAnnotations(); });

        builder.Services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        #endregion

        builder.Services.AddControllers();

        var app = builder.Build();

        // https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        #region -- Swagger and CORS --
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || st.SwaggerEnabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }

        // Use CORS
        var origins = st.Origins == null ? [] : st.Origins.Split(';');
        if (origins.Length > 0)
        {
            app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().WithOrigins(origins).AllowCredentials());
        }
        #endregion

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health");
        app.UseResponseCaching();
        app.MapGrpcService<GameParticipantServiceImpl>();
        app.MapHub<NotificationHub>("/notificationHub");
        app.MapHub<GameChatHub>("/gamechatHub");
        app.MapHub<PlaypalChatHub>("/playpalhub");

        app.Run();
    }

    #endregion

    #region -- Fields --

    /// <summary>
    /// Variable prefix
    /// </summary>
    private static string _prefix = "Ide";

    #endregion
}
