using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Wejo.Common.Core.Extensions;

/// <summary>
/// IServiceCollection extension for using [this IServiceCollection] only
/// </summary>
public static class IServiceCollectionExtension
{
    #region -- Methods --

    /// <summary>
    /// Add bearer authentication
    /// </summary>
    /// <param name="service">Service</param>
    /// <param name="jwt">JWT DTO</param>
    /// <returns>Return the result</returns>
    public static IServiceCollection AddBearerAuthentication(this IServiceCollection service, string? firebaseProjectId)
    {
        // JWT
        service.AddAuthentication(p =>
        {
            p.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            p.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(p =>
        {
            p.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
            p.RequireHttpsMetadata = false;
            p.SaveToken = true;
            p.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
                ValidAudience = firebaseProjectId,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // so tokens expire exactly at token expiration time (instead of 5 minutes later)
            };
            p.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Query.ContainsKey("access_token"))
                    {
                        context.Token = context.Request.Query["access_token"];
                    }
                    return Task.CompletedTask;
                }
            };
        });

        return service;
    }

    /// <summary>
    /// Extension method for registering Azure Blob Storage
    /// </summary>
    /// <param name="services">IServiceCollection instance</param>
    /// <param name="connectionString">Azure Blob Storage connection string</param>
    public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new BlobServiceClient(connectionString));
        return services;
    }

    /// <summary>
    /// Configures the maximum request body size and buffer size for IIS and Kestrel servers
    /// </summary>
    /// <param name="services">The IServiceCollection to configure</param>
    public static void ConfigureMaxRequestSizes(this IServiceCollection services)
    {
        var maxFileSize = 1024 * 1024 * 1024; // 1024MB
        var bufferSize = 10 * 1024 * 1024; // 10MB

        services.Configure<IISServerOptions>(p =>
        {
            p.MaxRequestBodySize = maxFileSize;
            p.MaxRequestBodyBufferSize = bufferSize;
        });

        services.Configure<KestrelServerOptions>(p =>
        {
            p.Limits.MaxRequestBodySize = maxFileSize;
            p.Limits.MaxRequestBufferSize = bufferSize;
        });

        services.Configure<FormOptions>(p =>
        {
            p.MultipartBodyLengthLimit = maxFileSize;
        });
    }

    #endregion
}
