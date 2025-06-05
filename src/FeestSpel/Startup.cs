using FeestSpel.Entities;
using FeestSpel.Middleware;
using InertiaCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeestSpel
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
            GameManager manager = new GameManager();
            manager.Start();

            services.AddSingleton<GameManager>(manager);

            services.AddWebOptimizer(pipeline =>
            {
                pipeline.MinifyJsFiles("**/*.js");
                pipeline.MinifyCssFiles("**/*.css");
            });

            services.AddRazorPages();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllers();
            services.AddProgressiveWebApp();

            services.AddInertia(options =>
            {
                options.RootView = "~/Pages/App.cshtml";
            });

            services.AddViteHelper(options =>
            {
                options.PublicDirectory = "wwwroot";
                options.BuildDirectory = "build";
                options.ManifestFilename = "manifest.json";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseWebOptimizer();
            }

            app.UseMiddleware<InertiaPropsMiddleware>();

            app.UseSession();

            app.UseStaticFiles();

            app.UseWebSockets(new WebSocketOptions() { KeepAliveInterval = TimeSpan.FromSeconds(150) });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            app.UseInertia();
        }
    }
}
