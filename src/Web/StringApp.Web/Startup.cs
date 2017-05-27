namespace StringApp.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.Webpack;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true).AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions { HotModuleReplacement = true });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Register the validation middleware, that is used to decrypt
            // the access tokens and populate the HttpContext.User property.
            app.UseOAuthValidation();

            // Register the OpenIddict middleware.
            app.UseOpenIddict();

            app.UseStaticFiles();

            app.UseMvc(
                routes =>
                    {
                        routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");

                        routes.MapSpaFallbackRoute(
                            name: "spa-fallback",
                            defaults: new { controller = "Home", action = "Index" });
                    });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddDbContext<DbContext>(
                options =>
                    {
                        // Configure the context to use an in-memory store.
                        options.UseInMemoryDatabase();

                        // Register the entity sets needed by OpenIddict.
                        // Note: use the generic overload if you need
                        // to replace the default OpenIddict entities.
                        options.UseOpenIddict();
                    });
            services.AddOpenIddict(
                options =>
                    {
                        // Register the Entity Framework stores.
                        options.AddEntityFrameworkCoreStores<DbContext>();

                        // Register the ASP.NET Core MVC binder used by OpenIddict.
                        // Note: if you don't call this method, you won't be able to
                        // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                        options.AddMvcBinders();

                        // Enable the token endpoint.
                        options.EnableTokenEndpoint("/connect/token");

                        // Enable the password flow.
                        options.AllowPasswordFlow();

                        // During development, you can disable the HTTPS requirement.
                        options.DisableHttpsRequirement();
                    });
        }
    }
}