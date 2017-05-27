namespace StringApp.Web
{
    using System.IO;

    using AspNet.Security.OpenIdConnect.Primitives;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.SpaServices.Webpack;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;

    using StringApp.Data;
    using StringApp.Data.Models;
    using StringApp.Services.Identity.Extensions;
    using StringApp.Services.Identity.Managers;

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory)
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

            app.AddAvgIdentityMigration<StringAppDbContext, User>(scopeFactory, Configuration);

            // Register the validation middleware, that is used to decrypt
            // the access tokens and populate the HttpContext.User property.
            app.UseOAuthValidation();

            // Register the OpenIddict middleware.
            app.UseOpenIddict();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
                                   {
                                       FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
                                       RequestPath = "/node_modules"
                                   });

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
            services.AddDbContext<StringAppDbContext>(
                options =>
                    {
                        // Configure the context to use an in-memory store.
                        options.UseSqlServer(this.Configuration.GetConnectionString("WebAppDb"));

                        // Register the entity sets needed by OpenIddict.
                        // Note: use the generic overload if you need
                        // to replace the default OpenIddict entities.
                        options.UseOpenIddict();
                    });

            services.AddScoped<DbContext, StringAppDbContext>();
            services.Add(ServiceDescriptor.Scoped(typeof(IUserRoleManager<,>), typeof(UserRoleManager<,>)));

            services.AddAvgIdentityServices<StringAppDbContext, User>(this.Configuration);

            services.Configure<IdentityOptions>(options =>
                {
                    options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                    options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                    options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
                });

            // Add framework services.
            services.AddMvc();

            services.AddOpenIddict(
                options =>
                    {
                        // Register the Entity Framework stores.
                        options.AddEntityFrameworkCoreStores<StringAppDbContext>();

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