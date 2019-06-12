using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StoreCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreCore.Interface
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        public DbInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //This example just creates an Administrator role and one Admin users
        public async void Initialize()
        {
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //create database schema if none exists
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                //If there is already an Administrator role, abort
                var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();
                if (!(await _roleManager.RoleExistsAsync("Admin")))
                {
                    //Create the Administartor Role
                    await _roleManager.CreateAsync(new ApplicationRole("Admin"));
                }
                //Create the default Admin account and apply the Administrator role
                var _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                string user = "Admin@admin.com";
                string password = "Admin@12345";
                var success = await _userManager.CreateAsync(new ApplicationUser { UserName = user, Email = user, EmailConfirmed = true }, password);
                if (success.Succeeded)
                {
                    await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(user), "Admin");
                }
            }
        }
    }
}
