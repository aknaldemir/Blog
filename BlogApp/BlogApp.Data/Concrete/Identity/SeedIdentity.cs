using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Entity.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApp.Data.Concrete.Identity
{
    public class SeedIdentity
    {
        public static async Task Seed(IApplicationBuilder app)
        {   
            var identityContext = app.ApplicationServices.GetRequiredService<ApplicationIdentityDbContext>();
            identityContext.Database.Migrate();  

            if (!identityContext.Roles.Any(i => i.Name == "Admin" && i.Name == "User"))
            {
                var roleStore = new RoleStore<IdentityRole>(identityContext);
                var roleManager = new RoleManager<IdentityRole>(roleStore, null, null, null, null);
               

                var adminRole = new IdentityRole { Name = "admin" };
                var userRole = new IdentityRole { Name = "user" };
                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(userRole);
                identityContext.SaveChanges();

                

               

            }

            if (!identityContext.Users.Any(i => i.UserName == "akinaldemir"))
            {
                var userStore = new UserStore<ApplicationUser>(identityContext);
                var userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);

                var admin = new ApplicationUser()
                {
                    UserName = "akinaldemir",
                    Email = "akinaldemir@gmail.com"
                };


                await userManager.CreateAsync(admin, "1234567");
                await userManager.AddToRoleAsync(admin, "admin");

                identityContext.SaveChanges();
            }
        }
    }
}
