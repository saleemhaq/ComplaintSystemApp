using System.Collections.Generic;
using System.Linq;
using ComplaintApp.Core.Users;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ComplaintApp.EntityFrameworkCore.Migrations
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                //Source: https://www.json-generator.com/

                var userData = System.IO.File.ReadAllText("Seed/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                // Add user Roles
                var roles = new List<Role>
                {
                    new Role { Name = "Customer" },
                    new Role { Name = "Admin" },
                    new Role { Name = "Staff" }
                };

                // Create the roles
                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                //string[] names = { "Admin", "Staff" };

                // Create the users
                foreach (var user in users) //.Where(x=> !names.Contains(x.UserName))
                {
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "Customer").Wait();
                }

                // We also need to create admin user which will have
                // access to all parts of our application
                var adminUser = new User
                {
                    UserName = "Admin"
                };

                var resultCreation = _userManager.CreateAsync(adminUser, "password").Result;

                if (resultCreation.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("Admin").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin", "Staff", "Customer" }).Wait();
                }
                var staffUser = new User
                {
                    UserName = "Staff"
                };
                var result = _userManager.CreateAsync(staffUser, "password").Result;

                if (result.Succeeded)
                {
                    var staff = _userManager.FindByNameAsync("Staff").Result;
                    _userManager.AddToRoleAsync(staff, "Staff").Wait();
                }
            }
        }
    }
}
