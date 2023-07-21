using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace NetBoard
{
    public class Program
    {
        public static string EnvironmentName =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        public static Action<IConfigurationBuilder> BuildConfiguration =
            builder => builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();


        // startup calls the web application
        public static int Main(string[] args)
        {
            Console.WriteLine("Starting NetBoard...");

            var builder = new ConfigurationBuilder();
            BuildConfiguration(builder);

            
            try
            {
                var hostBuilder = CreateHostBuilder(args, builder);

                var host = hostBuilder.Build();

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                // write to console 
                Console.WriteLine(ex.Message);
                // return
                return 1;
            }
        }
        // builds the web server
        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationBuilder configurationBuilder)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseIISIntegration()
                    .ConfigureKestrel(serverOptions =>
                    {
                        // Set properties and call methods on options.
                    })
                    .UseConfiguration(
                        configurationBuilder
                        .AddJsonFile("hosting.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"hosting.{EnvironmentName}.json", optional: true)
                        .Build()
                    )
                    .UseStartup<Startup>();
                });
        }
    }
}