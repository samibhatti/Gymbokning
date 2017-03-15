namespace Gymbokning_v0._1.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Gymbokning_v0._1.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { "Admin", "Editor" };
            foreach (var roleName in roleNames)
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    var role = new IdentityRole { Name = roleName };
                    var result = roleManager.Create(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var emails = new[] { "editor@editor.se", "admin@admin.se" };
            foreach (var email in emails)
            {
                if (!context.Users.Any(u => u.UserName == email))
                {
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                    };
                    var result = userManager.Create(user, "foobar");
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }

            var adminUser = userManager.FindByName("admin@admin.se");
            userManager.AddToRole(adminUser.Id, "Admin");

            var editorUser = userManager.FindByName("editor@editor.se");
            userManager.AddToRole(editorUser.Id, "Editor");                     

        }

    }
}
