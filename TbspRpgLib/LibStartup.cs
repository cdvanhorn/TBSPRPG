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
using TbspRpgLib.Aggregates;

using System;
using TbspRpgLib.InterServiceCommunication.Utilities;

namespace TbspRpgLib {
    public class LibStartup {
        public static void ConfigureTbspRpgServices(IConfiguration configuration, IServiceCollection services) {
            //services.AddScoped<IEventAdapter, EventAdapter>();

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
            
            services.AddScoped<IAggregateService, AggregateService>();
            services.AddScoped<IAggregateTypeRepository, AggregateTypeRepository>();
            services.AddScoped<IAggregateTypeService, AggregateTypeService>();

            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IServiceCommunication, ServiceCommunication>();
            services.AddScoped<IAdventureServiceLink, AdventureServiceLink>();
            services.AddScoped<IUserServiceLink, UserServiceLink>();
            services.AddScoped<IGameServiceLink, GameServiceLink>();
            services.AddScoped<IMapServiceLink, MapServiceLink>();
            services.AddScoped<IContentServiceLink, ContentServiceLink>();
        }

        public static void ConfigureTbspRpg(IApplicationBuilder app) {
            app.UseMiddleware<JwtMiddleware>();
        }
    }
}