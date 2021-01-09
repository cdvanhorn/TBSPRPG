using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

using TbspRpgLib.Jwt;
using TbspRpgLib.Settings;
using TbspRpgLib.Events;
using TbspRpgLib.Repositories;
using TbspRpgLib.Services;
using TbspRpgLib.InterServiceCommunication;

using System;

namespace TbspRpgLib {
    public class LibStartup {
        public static void ConfigureTbspRpgServices(IConfiguration configuration, IServiceCollection services) {
            //services.AddScoped<IEventAdapter, EventAdapter>();
            var connectionString = Environment.GetEnvironmentVariable("SERVICE_CONNECTION_STRING");

            if(connectionString != null && !string.IsNullOrWhiteSpace(connectionString)) {
                services.AddDbContext<ServiceContext>(
                    options => options.UseNpgsql(connectionString)
                );  
            }

            services.Configure<DatabaseSettings>(configuration.GetSection("Database"));
            services.AddSingleton<IDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
                
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddSingleton<IJwtSettings>(sp =>
                sp.GetRequiredService<IOptions<JwtSettings>>().Value);
            
            services.Configure<EventStoreSettings>(configuration.GetSection("EventStore"));
            services.AddSingleton<IEventStoreSettings>(sp =>
                sp.GetRequiredService<IOptions<EventStoreSettings>>().Value);

            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IServiceCommunication, ServiceCommunication>();
        }

        public static void ConfigureTbspRpg(IApplicationBuilder app) {
            app.UseMiddleware<JwtMiddleware>();
        }
    }
}