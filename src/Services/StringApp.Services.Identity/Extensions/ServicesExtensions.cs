namespace StringApp.Services.Identity.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using StringApp.Data.Models;
    using StringApp.Services.Identity.Exceptions;

    using OpenIddict;
    using OpenIddict.Models;

    public static class ServicesExtensions
    {
        public static void AddAvgIdentityServices<TContext, TUser>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TUser : User where TContext : IdentityDbContext<TUser>
        {
            PasswordOptions passwordOptions;

            try
            {
                passwordOptions = new PasswordOptions
                                      {
                                          RequireDigit =
                                              bool.Parse(
                                                  configuration[
                                                      "AvgIdentity:PasswordConfig:RequireDigit"]),
                                          RequireLowercase =
                                              bool.Parse(
                                                  configuration[
                                                      "AvgIdentity:PasswordConfig:RequireLowercase"]),
                                          RequireNonAlphanumeric =
                                              bool.Parse(
                                                  configuration[
                                                      "AvgIdentity:PasswordConfig:RequireNonAlphanumeric"]),
                                          RequireUppercase =
                                              bool.Parse(
                                                  configuration[
                                                      "AvgIdentity:PasswordConfig:RequireUppercase"]),
                                          RequiredLength =
                                              int.Parse(
                                                  configuration[
                                                      "AvgIdentity:PasswordConfig:RequiredLength"])
                                      };
            }
            catch
            {
                throw new ConfigurationException("AvgIdentity PasswordConfig error");
            }

            services.AddIdentity<TUser, IdentityRole>(o => o.Password = passwordOptions)
                .AddEntityFrameworkStores<TContext>().AddDefaultTokenProviders();
        }
    }
}