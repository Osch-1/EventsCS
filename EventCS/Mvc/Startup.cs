using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvc.Application;
using Mvc.Application.EventsHandler;
using Mvc.Application.JsonCreator;
using Mvc.Data.Repositories;

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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<IEventRepository>(s => new SQLEventRepository(Configuration.GetConnectionString("LocalEventDb")));//введение зависимости, каждый раз при вызове IEventReopsitory будет обращение к SQLEventReopsitory
            services.AddScoped<IJsonCreator, PlainJsonCreator>();
            services.AddTransient<IEventsHandler>(s => new LogEventsHandler(new SQLEventRepository(Configuration.GetConnectionString("LocalEventDb"))));

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();// чтобы видеть ошибки в процессе разработки
            app.UseStatusCodePages();// отображать код запроса
            app.UseStaticFiles();// отображать css
            app.UseRouting(); // используем систему маршрутизации

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}");
            });
        }
    }
}
