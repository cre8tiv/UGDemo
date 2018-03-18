using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using DemoProject.Data;
using Swashbuckle.AspNetCore.Swagger;
using DemoProject.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DemoProject
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
            ConfigureDatabase(services);

            // configure redis only when not using develop
            ConfigureRedis(services);

            services
                .AddMvc()
                .AddJsonOptions(json => json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);
        }

        public void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<DemoProjectContext>(options =>
                options.UseSqlServer(GetConnectionString()));
        }

        public void ConfigureRedis(IServiceCollection services)
        {
            var redis = ConnectionMultiplexer.Connect(TryResolveDns(Configuration["ConnectionStrings:RedisSessionConnection"]));
            services.AddDataProtection()
                .SetApplicationName("DemoProject.API")
                .PersistKeysToRedis(redis, "DataProtectionKeys");
        }

        

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                loggerFactory.AddConsole();

            app.UseCors("CorsPolicy");

            //support for running in environments other than develop
            if (!env.IsDevelopment())
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseAuthentication();

            app.UseMvc();
        }

        public virtual string GetConnectionString()
        {
            if (Environment.GetEnvironmentVariable("USE_DOCKER_FOR_DEV") == "False" || Environment.GetEnvironmentVariable("USE_DOCKER_FOR_DEV") == null)
            {
                return Configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                return Configuration.GetConnectionString("DockerConnection");
            }
        }

        public static string TryResolveDns(string redisUrl)
        {
            var hostName = redisUrl.Substring(0, redisUrl.IndexOf(":"));
            var isIp = IsIpAddress(hostName);
            if (!isIp)
            {
                var ip = Dns.GetHostEntryAsync(hostName).GetAwaiter().GetResult();
                var resolvedIp = ip.AddressList.First(x => IsIpAddress(x.ToString())).ToString();
                return redisUrl.Replace(redisUrl.Substring(0, redisUrl.IndexOf(":")), resolvedIp);
            }
            return redisUrl;
        }

        private static bool IsIpAddress(string host)
        {
            return Regex.IsMatch(host, @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
        }
    }
}