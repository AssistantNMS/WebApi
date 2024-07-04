using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AssistantApps.NoMansSky.Info;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NMS.Assistant.Api.Helper;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Domain.Configuration;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Persistence;

namespace NMS.Assistant.Api
{
    public class Startup
    {
        public IApiConfiguration ApiConfiguration { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            ApiConfiguration = Configuration.Get<ApiConfiguration>();

            services.RegisterCommonServices(ApiConfiguration);
            services.RegisterThirdPartyServicesForApi(ApiConfiguration);

            bool isProd = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty)
                .Equals("production", StringComparison.InvariantCultureIgnoreCase);
            string binFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
            string prodBinFolder = Path.Combine(isProd ? string.Empty : binFolderPath, "Assets");
            Console.WriteLine($"binFolderPath: {binFolderPath}");
            Console.WriteLine($"prodBinFolder: {prodBinFolder}");
            services.AddNoMansSkyInfo(prodBinFolder);

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            services.AddCors(options =>
            {
                options.AddPolicy(ApiCorsSettings.DefaultCorsPolicy, builder => 
                    builder.WithOrigins(ApiConfiguration.AllowedHosts.ToArray())
                    .WithMethods(ApiCorsSettings.AllowedMethods)
                    .WithHeaders(ApiCorsSettings.ExposedHeaders)
                    .WithExposedHeaders(ApiCorsSettings.ExposedHeaders)
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(30))
                    .AllowCredentials()
                );
                options.AddPolicy(ApiCorsSettings.CorsAllowAll, builder =>
                    builder.SetIsOriginAllowed(hostName => true)
                    .WithMethods(ApiCorsSettings.AllowedMethods)
                    .WithHeaders(ApiCorsSettings.ExposedHeaders)
                    .WithExposedHeaders(ApiCorsSettings.ExposedHeaders)
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(30))
                    .AllowCredentials()
                );
            });
            services.AddRouting();
            services.AddMvc().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, NmsAssistantContext db)
        {
            if (env.IsDevelopment())
            {
                //app.UseDatabaseErrorPage();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            db.Database.Migrate();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{ApiAccess.PublicBasic}/swagger.json", "AssistantNMS API - Public Basic");
                c.SwaggerEndpoint($"/swagger/{ApiAccess.Public}/swagger.json", "AssistantNMS API - Public Advanced");
                c.SwaggerEndpoint($"/swagger/{ApiAccess.Auth}/swagger.json", "AssistantNMS API - Authenticated");
                c.SwaggerEndpoint($"/swagger/{ApiAccess.All}/swagger.json", "AssistantNMS API - All");
                //c.InjectStylesheet("/assets/css/customSwagger.css");
                c.DocumentTitle = "AssistantNMS API Documentation";
                c.RoutePrefix = string.Empty;
                c.DisplayRequestDuration();
            });

            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() => Task.WhenAll(
                    context.Response.ApplyAuthorHeader(),
                    context.Response.ApplyDefaultCacheHeader(),
                    context.Response.ApplySponsoredByHeaders(),
                    context.Response.ApplyDefaultResponseFromCacheHeader()
                ));
                await next();
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCors(ApiCorsSettings.DefaultCorsPolicy);
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
