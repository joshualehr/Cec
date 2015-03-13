using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Cec.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    // When the User Logs in, you can display the profile information by doing the following:
    // Get the current logged in UserId, so you can look the user up in ASP.NET Identity system 
    //    var currentUserId = User.Identity.GetUserId(); 
    // Instantiate the UserManager in ASP.Identity system so you can look up the user in the system 
    //    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())); 
    // Get the User object 
    //    var currentUser = manager.FindById(User.Identity.GetUserId()); 
    // Get the profile information about the user 
    //    currentUser.Contact.FirstName 
    public class ApplicationUser : IdentityUser
    {
        public Guid ContactID { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema:false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<AreaMaterial> AreaMaterials { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<ModelMaterial> ModelMaterials { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
    }

    public class CecInitializer : System.Data.Entity.CreateDatabaseIfNotExists<ApplicationDbContext>
    {
    }
}