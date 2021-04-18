using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Models.Data
{
    public class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<User> userManager)
        {
            var user = userManager.FindByEmailAsync("admin@admin.com").Result;
            if (user == null)
            {
                user = new User
                {
                    FullName = "admin",
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Status = AccountStatus.Active
                };

                IdentityResult result = userManager.CreateAsync(user, "Admin@123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
            else
            {
                var userRoleExist = userManager.IsInRoleAsync(user, "Administrator").Result;
                if (!userRoleExist)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

    }
}
