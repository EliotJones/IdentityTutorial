﻿namespace IdentityTutorial.Web
{
    using System.IO;
    using Core;
    using IConfiguration = Microsoft.Framework.ConfigurationModel.IConfiguration;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Diagnostics;
    using Microsoft.AspNet.Hosting;
    using Microsoft.Framework.ConfigurationModel;
    using Microsoft.Framework.DependencyInjection;
    using Microsoft.Framework.Logging;
    using Microsoft.Framework.Runtime;
    using Store;

    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.
            var config = new Configuration(appEnv.ApplicationBasePath)
                // Only available if Microsoft.Framework.ConfigurationModel.Json is referenced in project.json
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = config;
            this.appEnv = appEnv;
        }

        public IConfiguration Configuration { get; set; }
        public IApplicationEnvironment appEnv;
        
        /// <summary>
        /// Sets up services for dependency injection across the application.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSubKey("AppSettings"));

            // Only generate a single instance of the context due to the way it's designed.
            services.AddSingleton(_ => 
            Context.Get(new FileReader(Path.Combine(appEnv.ApplicationBasePath, "App_Data", "test.txt"))));

            services.AddMvc();

            // Add identity stores.
            services
                .AddIdentity<CustomUser, CustomRole>()
                .AddDefaultTokenProviders()
                .AddUserStore<CustomUserStore>()
                .AddRoleStore<CustomRoleStore>();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Configure the HTTP request pipeline.

            // Add the console logger.
            loggerfactory.AddConsole();

            // Add the following to the request pipeline only in development environment.
            if (env.IsEnvironment("Development"))
            {
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }
            
            app.UseStaticFiles()
                .UseIdentity();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}