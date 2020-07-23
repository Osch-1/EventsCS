using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mvc.Application.Interfaces;

namespace Mvc
{
    public class Program
    {
        public static void Main( string[] args )
        {            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder( string[] args) =>
            WebHost.CreateDefaultBuilder( args )
                .UseStartup<Startup>();
    }
}
