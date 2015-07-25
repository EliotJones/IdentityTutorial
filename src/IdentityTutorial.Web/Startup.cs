namespace IdentityTutorial.Web
{
    using System.IO;
    using Core;
    using Data;
    using IConfiguration = Microsoft.Framework.Configuration.IConfiguration;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Diagnostics;
    using Microsoft.AspNet.Hosting;
    using Microsoft.Framework.Configuration;
    using Microsoft.Framework.DependencyInjection;
    using Microsoft.Framework.Logging;
    using Microsoft.Framework.Runtime;

    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.
            var config = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
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
            services.Configure<AppSettings>(Configuration, "AppSettings");
            
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

            var appId = Configuration.GetConfigurationSection("AppSettings").Get("FacebookId");
            var appSecret = Configuration.GetConfigurationSection("AppSettings").Get("FacebookSecret");

            // Add Facebook logins
            services.ConfigureFacebookAuthentication(options =>
            {
                options.AppId = appId;
                options.AppSecret = appSecret;
            });

            services.ConfigureIdentity(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.User.UserNameValidationRegex = @"^[\d\w\s@\-\.]+$";
                options.Lockout.MaxFailedAccessAttempts = 3;
            });

            services.AddCors();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Configure the HTTP request pipeline.
            app.UseCors(p =>
            {
                p.WithOrigins("https://maxcdn.bootstrapcdn.com");
                p.AllowAnyMethod();
                p.AllowAnyHeader();
            });

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
                .UseIdentity()
                .UseFacebookAuthentication();

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
