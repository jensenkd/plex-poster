using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plex.Api;
using Plex.Api.Api;
using PlexPoster.Api.Hubs;
using PlexPoster.Api.Services;

namespace PlexPoster.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        private readonly string CorsOptions = "CorsOptions";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup Client Options
            var clientOptions = new ClientOptions
            {
                ApplicationName = "PlexPoster",
                DeviceName = "PlexPoster",
                ClientId = Guid.Parse("0b51dda7-ccbf-4f0e-bac5-85f1024b56da")
            };
            
            // Setup CORS
            services.AddCors(options =>
            {
                options.AddPolicy(CorsOptions,
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:8080", "http://localhost:8001")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
        
            // Setup DI
            services.AddLogging();
            services.AddSingleton<IApiService, ApiService>();
            services.AddTransient<IPlexClient, PlexClient>();
            services.AddTransient<PlexService, PlexService>();
            services.AddTransient<IPlexRequestsHttpClient, PlexRequestsHttpClient>();
            services.AddSingleton<ClientOptions>(clientOptions);

            services.AddSignalR();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable UseCors with named policy.
            app.UseCors(CorsOptions);
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SessionHub>("/hubs/session");
            });
        }
    }
}