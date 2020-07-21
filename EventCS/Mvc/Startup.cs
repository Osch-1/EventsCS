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
using System;
using System.Collections.Generic;
using System.IO;

namespace Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddMvc();

            services.AddTransient<IEventRepository>( s => new SQLEventRepository( Configuration.GetConnectionString( "LocalEventDb" ) ) );//введение зависимости, каждый раз при вызове IEventReopsitory будет обращение к SQLEventReopsitory
            services.AddScoped<IEventCreator, PlainEventCreator>();
            services.AddTransient<IEventsManager>( s => new EventsManager( new SQLEventRepository( Configuration.GetConnectionString( "LocalEventDb" ) ) ) );

            JToken jAppSettings = JToken.Parse( File.ReadAllText( Path.Combine( Environment.CurrentDirectory, "appsettings.json" ) ) );            
            var rabbitMQEventBusSettings = new RabbitMQEventBusSettings
            {
                Service = jAppSettings["RabbitMQEventBusSettings"]["Service"].ToString(),
                RetryMessageProcessingSettings = JsonConvert.DeserializeObject<RetryMessageProcessingSettings>( jAppSettings["RabbitMQEventBusSettings"]["RetryMessageProcessingSettings"].ToString() ),
                ConnectionSettings = JsonConvert.DeserializeObject<RabbitMQConnectionSettings>( jAppSettings["RabbitMQEventBusSettings"]["ConnectionSettings"].ToString() )
            };
            var connectionSettings = rabbitMQEventBusSettings.ConnectionSettings;

            services.AddTransient<IRabbitMQPersistentConnection>( s => new RabbitMQPersistentConnection( RabbitMQPersistentConnection.CreateConnectionFactory( connectionSettings ), rabbitMQEventBusSettings ) );
            services.AddScoped<IEventsReceiver, RabbitEventsReceiver>();

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
            //app.UseExceptionHandler("/Events/Error");
            //app.UseHsts();
            app.UseStaticFiles();// отображать css
            app.UseRouting(); // используем систему маршрутизации    
            app.UseStatusCodePages();

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}");
            });
        }
    }
}
