using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DemoProject.Data;

namespace DemoProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DemoProjectContext>();
                    DbInit.Initialize(context);
                    DbSeed.SeedTestData(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
                .Build();

            if (env == "Staging" || env == "Production")
            {
                var certificateSettings = config.GetSection("CertificateSettings");
                string certificateFileName = certificateSettings.GetValue<string>("filename");
                string certificatePassword = certificateSettings.GetValue<string>("password");
                var cert = new X509Certificate2(certificateFileName, certificatePassword);

                return WebHost.CreateDefaultBuilder(args)
                    .UseApplicationInsights()
                    .UseStartup<Startup>()
                    .UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 5000);
                        options.Listen(IPAddress.Any, 5001, listenOptions =>
                        {
                            listenOptions.UseHttps(cert);
                        });
                    })
                    .Build();
            }
            else
            {
                return WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .CaptureStartupErrors(true)
                    .UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 9080);
                    })
                    .Build();
            }
        }
    }
}
