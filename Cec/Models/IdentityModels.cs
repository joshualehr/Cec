using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Cec.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Guid ContactID { get; set; }

        public string AllRoles { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<ToDo> ToDos { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<AreaMaterial> AreaMaterials { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<ModelMaterial> ModelMaterials { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public virtual DbSet<ProjectContact> ProjectContacts { get; set; }
        public virtual DbSet<ToDo> ToDos { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema:false) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>()
                .HasOptional(e => e.ParentToDo)
                .WithMany(e => e.ChildToDos)
                .HasForeignKey(e => e.ParentToDoID);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    public class CecInitializer : CreateDatabaseIfNotExists<ApplicationDbContext> { }
}