using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Persistence;
using Microsoft.OpenApi.Models;
using NMS.Assistant.Data.Filter;
using NMS.Assistant.Domain.Constants;
using StackExchange.Redis;

namespace NMS.Assistant.Data.Helper
{
    public static class ThirdPartySetupHelper
    {
        public static IServiceCollection RegisterThirdPartyServicesForApi(this IServiceCollection services, IApiConfiguration config)
        {
            services.SetUpApplicationInsight(config);
            services.SetUpEntityFramework(config);
            services.SetUpJwt(config);
            services.SetUpSwagger();
            services.SetUpRedis(config);
            return services;
        }

        public static IServiceCollection RegisterThirdPartyServicesForConsoleApp(this IServiceCollection services, IApiConfiguration config)
        {
            services.SetUpEntityFramework(config);
            services.SetUpRedis(config);
            return services;
        }

        //private static void SetUpStripe(this IServiceCollection services, IApiConfiguration config)
        //{
        //    StripeConfiguration.ApiKey = config.Stripe.SecretKey;
        //}

        private static void SetUpApplicationInsight(this IServiceCollection services, IApiConfiguration config)
        {
            if (config.ApplicationInsights.Enabled)
            {
                services.AddApplicationInsightsTelemetry((applicationInsightsConfig) => {
                    applicationInsightsConfig.ConnectionString = config.ApplicationInsights.ConnectionString;
                });
            }
        }

        private static void SetUpEntityFramework(this IServiceCollection services, IApiConfiguration config)
        {
            services.AddDbContext<NmsAssistantContext>(options => options
                .UseLazyLoadingProxies()
                .UseSqlServer(config.Database.ConnectionString, dbOptions => dbOptions.MigrationsAssembly("NMS.Assistant.Api"))
            );
        }

        private static void SetUpRedis(this IServiceCollection services, IApiConfiguration config)
        {
            //services.AddStackExchangeRedisCache(option =>
            //{
            //    option.Configuration = "127.0.0.1:6379";
            //    option.InstanceName = "master";
            //    option.ConfigurationOptions = new ConfigurationOptions
            //    {
            //        ConnectTimeout = 5000
            //    };
            //});
        }

        private static void SetUpJwt(this IServiceCollection services, IApiConfiguration config)
        {

            byte[] key = Encoding.ASCII.GetBytes(config.Jwt.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    //x.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.FromSeconds(config.Jwt.ClockSkewInSeconds)
                    };
                });
        }

        private static void SetUpSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiAccess.PublicBasic, SwaggerHelper.CreateInfoForApiVersion(ApiAccess.PublicBasic));
                c.SwaggerDoc(ApiAccess.Public, SwaggerHelper.CreateInfoForApiVersion(ApiAccess.Public));
                c.SwaggerDoc(ApiAccess.Auth, SwaggerHelper.CreateInfoForApiVersion(ApiAccess.Auth));
                c.SwaggerDoc(ApiAccess.All, SwaggerHelper.CreateInfoForApiVersion(ApiAccess.All));
                c.DocInclusionPredicate(SwaggerHelper.DocInclusionPredicate);

                c.AddSecurityDefinition(ApiAuthScheme.Basic, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Description = "basic authentication for API",
                    In = ParameterLocation.Header,
                    Scheme = ApiAuthScheme.Basic
                });
                c.AddSecurityDefinition(ApiAuthScheme.JwtBearer, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                                  "Enter 'Bearer' [space] and then your token in the text input below." +
                                  "\r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = ApiAuthScheme.JwtBearer
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();

                // Set the comments path for the Swagger JSON and UI.
                const string xmlFile = "NMS.Assistant.Api.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
