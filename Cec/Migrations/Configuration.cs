using Cec.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Cec.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Cec.Models.ApplicationDbContext";
        }

        bool AddUserAndRole(ApplicationDbContext context)
        {
            IdentityResult ir;
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var user = new ApplicationUser() { UserName = "JoshuaLehr", Contact = new Contact() { FirstName = "Joshua", LastName = "Lehr" } };
            ir = um.Create(user, "j040375l");
            if (ir.Succeeded == false)
                return ir.Succeeded;
            string[] roles = { "canAdminister", "canViewDetails", "isEmployee" };
            foreach (var item in roles)
            {
                if (!rm.RoleExists(item))
                {
                    ir = rm.Create(new IdentityRole(item));
                    ir = um.AddToRole(user.Id, item);
                    if (ir.Succeeded == false)
                        return ir.Succeeded;
                }
            }
            return ir.Succeeded;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            if (AddUserAndRole(context))

            {
                context.Projects.AddOrUpdate(p => p.Designation,
                  new Project
                  {
                      ProjectID = Guid.NewGuid(),
                      Designation = "Covered Bridge",
                      Description = "apartmant renovation",
                      User = context.Users.Single(u => u.UserName == "JoshuaLehr")
                  },
                  new Project
                  {
                      ProjectID = Guid.NewGuid(),
                      Designation = "Quail Run",
                      Description = "pool demolition",
                      User = context.Users.Single(u => u.UserName == "JoshuaLehr")
                  },
                  new Project
                  {
                      ProjectID = Guid.NewGuid(),
                      Designation = "Union Street Flats",
                      Description = "new apartment complex",
                      User = context.Users.Single(u => u.UserName == "JoshuaLehr")
                  }
                );
            }
        }
    }
}
