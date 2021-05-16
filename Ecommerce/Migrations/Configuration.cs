namespace Ecommerce.Migrations
{
    using Ecommerce.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Ecommerce.Models.ApplicationDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Ecommerce.Models.ApplicationDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            ApplicationDBContext db = new ApplicationDBContext();
            ApplicationUserManager manager = new ApplicationUserManager(db);

            var user = new ApplicationIdentityUser { UserName = "admin", Email = "example.gmail.com", Gender = Gender.Male };
            manager.Create(user, "12345678");

            var roleManger = new ApplicationRoleManager(db);
            roleManger.Create(new IdentityRole("Admin"));

            manager.AddToRole(user.Id, "Admin");
        }
    }
}
