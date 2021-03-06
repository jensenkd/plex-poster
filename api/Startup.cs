using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Plex.Api;
using Plex.Api.Api;
using Plex.Api.Clients;
using Plex.Api.Clients.Interfaces;
using Plex.Api.Factories;
using PlexPoster.Api.Hubs;
using PlexPoster.Api.Services;

namespace PlexPoster.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly string CorsOptions = "CorsOptions";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
      
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup Client Options
            var clientOptions = new ClientOptions
            {
                Platform = "Web",
                Product = "API_UnitTests",
                DeviceName = "API_UnitTests",
                ClientId = "PlexApi",
                Version = "v1",
            };
            
            // Setup DI
            services.AddLogging();
            services.AddSingleton(clientOptions);
            services.AddTransient<IPlexServerClient, PlexServerClient>();
            services.AddTransient<IPlexAccountClient, PlexAccountClient>();
            services.AddTransient<IPlexLibraryClient, PlexLibraryClient>();
            services.AddTransient<IApiService, ApiService>();
            services.AddTransient<IPlexFactory, PlexFactory>();
            services.AddTransient<IPlexRequestsHttpClient, PlexRequestsHttpClient>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlexPoster.Api", Version = "v1" });
            });
        }

       public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlexPoster.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}