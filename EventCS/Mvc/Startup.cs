using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvc.Application;
using Mvc.Application.EventsHandler;
using Mvc.Application.Interfaces;
using Mvc.Application.JsonCreator;
using Mvc.Data.Repositories;
using Mvc.Infrastructure.EventsReceivers.RabbitMQEventsReceiver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mvc
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddMvc();

            //Configs
            services.AddScoped<IEventRepository>( s => new SQLEventRepository( Configuration.GetConnectionString( "LocalEventDb" ) ) );
            services.AddScoped<IEventsManager>( s => new LogEventsManager( new SQLEventRepository( Configuration.GetConnectionString( "LocalEventDb" ) ) ) );
            services.AddScoped<IEventCreator, PlainEventCreator>();

            services.AddSingleton( Configuration.GetSection( "RabbitMQConnectionSettings" ).Get<RabbitMQConnectionSettings>() );
            services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
            {
                return RabbitMQPersistentConnection.CreateConnectionFactory( sp.GetService<RabbitMQConnectionSettings>() );
            });
            services.AddSingleton<IRabbitMQPersistentConnection, RabbitMQPersistentConnection>();

            services.AddSingleton<IEventsReceiver, RabbitEventsReceiver>();            
            
            services.Configure<FormOptions>( o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Events/Error");
            app.UseHsts();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseStatusCodePages();

            var eventsReceiver = app.ApplicationServices.GetService<IEventsReceiver>();
            eventsReceiver.Init();

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}");
            });
        }
    }
}
