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

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatabase(services);

            services.AddMvc();
        }

        public void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<DemoProjectContext>(options =>
                options.UseSqlServer(GetConnectionString()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                loggerFactory.AddConsole();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
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

    }
}