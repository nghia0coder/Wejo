﻿using Microsoft.EntityFrameworkCore;

namespace Wejo.Game.API;

using Application;
using Application.Extensions;
using Application.Interfaces;
using Application.Services;
using Common.Core.Extensions;
using Common.Core.Protos;
using Common.Domain.Database;
using Common.Domain.Interfaces;
using Common.SeedWork.Extensions;
using Infrastructure.MessageQueue;
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

        // Load configuration environment
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

        #region -- Setup DI --
        // Setting
        builder.Services.AddSingleton<ISetting>(st!);

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        // GameChat Service
        builder.Services.AddScoped<IGameChatService, GameChatService>();

        // Redis
        builder.Services.AddRedis(builder.Configuration);
        builder.Services.AddScoped<IUserCacheService, UserCacheService>();

        // Message Queue
        builder.Services.AddSingleton<IMessageQueue, RabbitMQProducer>();

        // DbContext
        builder.Services.AddDbContext<WejoContext>(p => p.UseNpgsql(csDb!, p => p.MigrationsAssembly(assembly).EnableRetryOnFailure()), ServiceLifetime.Scoped);
        builder.Services.AddScoped<IWejoContext>(p => p.GetService<WejoContext>()!);

        // Cassandra
        builder.Services.AddCassandra(builder.Configuration);

        // MediatR
        builder.Services.AddMediatR(p =>
        {
            p.RegisterServicesFromAssembly(me.Assembly);

            p.AddDiGame();
            p.AddDiGameChat();
            p.AddDiGameParticipant();
        });
        #endregion

        // gRPC Client
        builder.Services.AddGrpcClient<GameParticipantService.GameParticipantServiceClient>(o =>
        {
            o.Address = new Uri("http://wejo_realtime_service:5001");
        }).ConfigureChannel(o =>
        {
            o.HttpHandler = new SocketsHttpHandler
            {
                EnableMultipleHttp2Connections = true,
                KeepAlivePingDelay = TimeSpan.FromSeconds(30)
            };
        });

        #region -- Setup token --
        var firebaseProjectId = builder.Configuration["Firebase:ProjectId"];
        builder.Services.AddBearerAuthentication(firebaseProjectId);
        builder.Services.AddResponseCaching();

        // Cookie name
        builder.Services.ConfigureApplicationCookie(p => { p.Cookie.Name = _prefix; });
        #endregion

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(p => { p.EnableAnnotations(); });

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

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health");
        app.UseResponseCaching();

        // Explicitly set the API to listen on port 8081
        app.Run("http://0.0.0.0:8081");
    }

    #endregion

    #region -- Fields --

    /// <summary>
    /// Variable prefix
    /// </summary>
    private static string _prefix = "Ide";

    /// <summary>
    /// Media extension allow
    /// </summary>
    public static string _mediaExtensionAllow = default!;

    #endregion
}
