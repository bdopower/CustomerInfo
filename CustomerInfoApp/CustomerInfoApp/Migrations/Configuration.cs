namespace CustomerInfoApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CustomerInfoApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CustomerInfoApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            ApplicationUser adminUser = new ApplicationUser()
            {
                Email = "admin@bdo.com",
                UserName = "admin@bdo.com",                
            };
            ApplicationUser user = new ApplicationUser()
            {
                Email = "user@bdo.com",
                UserName = "user@bdo.com"
            };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));

            userManager.Create(adminUser, "123456");
            userManager.Create(user, "123456");

            userManager.AddToRole(adminUser.Id, "Admin");
            userManager.AddToRole(user.Id, "User");
        }
    }
}
