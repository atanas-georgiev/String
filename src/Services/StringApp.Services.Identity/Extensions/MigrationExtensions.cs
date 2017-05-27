namespace StringApp.Services.Identity.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using StringApp.Data.Models;
    using StringApp.Services.Identity.Exceptions;
    using StringApp.Services.Identity.Managers;

    public static class MigrationExtensions
    {
        public static async Task AddAvgIdentityMigration<TContext, TUser>(
            this IApplicationBuilder app,
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
            where TUser : User, new() where TContext : IdentityDbContext<TUser>
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TContext>();
                var userRoleManager = serviceScope.ServiceProvider.GetService<IUserRoleManager<TUser, TContext>>();

                try
                {
                    if (!context.Roles.Any())
                    {
                        await SeedRoles(userRoleManager, configuration);
                        await SeedUsers(userRoleManager, configuration);
                    }
                }
                catch
                {
                    context.Database.Migrate();

                    if (!context.Roles.Any())
                    {
                        await SeedRoles(userRoleManager, configuration);
                        await SeedUsers(userRoleManager, configuration);
                    }
                }
            }
        }

        private static async Task SeedRoles<TUser, TContext>(
            IUserRoleManager<TUser, TContext> userRoleManager,
            IConfiguration configuration)
            where TUser : User, new() where TContext : IdentityDbContext<TUser>
        {
            var roles = new List<string>();

            try
            {
                roles = configuration.GetSection("AvgIdentity").GetSection("InitialData").GetSection("Roles")
                    .GetChildren().Select(c => c.Value).ToList();
            }
            catch
            {
                throw new ConfigurationException("AvgIdentity InitialData error");
            }

            await userRoleManager.AddRoleAsync(roles);
        }

        private static async Task SeedUsers<TUser, TContext>(
            IUserRoleManager<TUser, TContext> userRoleManager,
            IConfiguration configuration)
            where TUser : User, new() where TContext : IdentityDbContext<TUser>
        {
            try
            {
                // TODO: add reflection
                // var userref = typeof(TUser).GetTypeInfo().GetProperties();
                var users = configuration.GetSection("AvgIdentity").GetSection("InitialData").GetSection("Users")
                    .GetChildren().Select(
                        c => new
                                 {
                                     FirstName = c.GetSection("FirstName").Value,
                                     LastName = c.GetSection("LastName").Value,
                                     Email = c.GetSection("Email").Value,
                                     Password = c.GetSection("Password").Value,
                                     Role = c.GetSection("Role").Value
                                 });

                foreach (var user in users)
                {
                    await userRoleManager.AddUserAsync(
                        user.Email,
                        user.Password,
                        user.FirstName,
                        user.LastName,
                        user.Role);
                }
            }
            catch (Exception)
            {
                throw new ConfigurationException("AvgIdentity InitialData error");
            }
        }
    }
}