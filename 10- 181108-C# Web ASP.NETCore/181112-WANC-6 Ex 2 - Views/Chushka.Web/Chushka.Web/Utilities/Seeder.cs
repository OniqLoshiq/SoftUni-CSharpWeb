using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Chushka.Web.Utilities
{
    public static class Seeder
    {
        private static string[] roles = { "Admin", "User" };
        
        public static void Seed(IServiceProvider provider)
        {
            var roleManager = provider.GetService<RoleManager<IdentityRole>>();

            foreach (var roleName in roles)
            {
                Task<bool> roleExist = roleManager.RoleExistsAsync(roleName);
                roleExist.Wait();

                if(!roleExist.Result)
                {
                    Task<IdentityResult> roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
                    roleResult.Wait();
                }
            }
        }
    }
}
