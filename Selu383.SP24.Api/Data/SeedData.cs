using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Features.Hotels;
using Selu383.SP24.Api.Features.Users;
using System.Security.Claims;

namespace Selu383.SP24.Api.Data
{
    public class SeedData
    {
        public static async Task MigrateAndSeed(IServiceProvider serviceProvider)
        {
            var dataContext = serviceProvider.GetRequiredService<DataContext>();

            await dataContext.Database.MigrateAsync();

            await AddRoles(serviceProvider);
            await AddUsers(serviceProvider);

            var hotels = dataContext.Set<Hotel>();

            if (!await hotels.AnyAsync()) //moved this from project.cs to here 
            {
                for (int i = 0; i < 6; i++)
                {
                    dataContext.Set<Hotel>()
                        .Add(new Hotel
                        {
                            Name = "Hammond " + i,
                            Address = "1234 Place st",
                        });
                }

                await dataContext.SaveChangesAsync();
            }
        }

        private static async Task AddUsers(IServiceProvider serviceProvider)
        {
            const string defaultPass = "Password123!";
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            if (await userManager.Users.AnyAsync())
            {
                return;
            }

            var adminUser = new User
            {
                UserName = "galkadi",
            };

            var result = await userManager.CreateAsync(adminUser, defaultPass);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
            }

            var bob = new User
            {
                UserName = "bob",
            };

            await userManager.CreateAsync(bob, defaultPass);
            await userManager.AddToRoleAsync(bob, RoleNames.User);

            var sue = new User
            {
                UserName = "sue",
            };

            await userManager.CreateAsync(sue, defaultPass);
            await userManager.AddToRoleAsync(sue, RoleNames.User);
        }


        private static async Task AddRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            if (roleManager.Roles.Any())
            {
                return;
            }

            await roleManager.CreateAsync(new Role
            {
                Name = RoleNames.Admin,
            });
            await roleManager.CreateAsync(new Role
            {
                Name = RoleNames.User,
            });
        }
    }
}
