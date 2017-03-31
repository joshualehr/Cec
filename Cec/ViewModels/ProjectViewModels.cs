using Cec.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Cec.ViewModels
{
    public class ProjectIndexViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        public string Project { get; set; }

        //Constructors
        public ProjectIndexViewModel() { }

        //Methods
        public List<ProjectIndexViewModel> ListAll()
        {
            List<ProjectIndexViewModel> projects = db.Projects.OrderBy(p => p.Designation)
                .OrderBy(p => p.Designation)
                .Select(p => new ProjectIndexViewModel { ProjectId = p.ProjectID, Project = p.Designation })
                .ToList();
            return projects;
        }

        public List<ProjectIndexViewModel> ListByManager(string userId)
        {
            List<ProjectIndexViewModel> projects = db.Projects.Where(p => p.User.Id == userId)
                .OrderBy(p => p.Designation)
                .Select(p => new ProjectIndexViewModel { ProjectId = p.ProjectID, Project = p.Designation })
                .ToList();
            return projects;
        }

        public List<ProjectIndexViewModel> ListByEmployee(string userId)
        {
            Guid contactId = db.Users.Single(u => u.Id == userId).Contact.ContactID;
            List<ProjectIndexViewModel> projects = db.ProjectContacts.Where(p => p.ContactID == contactId)
                .Select(p => p.Project)
                .OrderBy(p => p.Designation)
                .Select(p => new ProjectIndexViewModel { ProjectId = p.ProjectID, Project = p.Designation })
                .ToList();
            return projects;
        }
    }

    public class ProjectStatusViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Guid BuildingId { get; set; }
        public string Building { get; set; }
        public int AreaCount { get; set; }
        public IDictionary<string, double> Statuses { get; set; }

        public ProjectStatusViewModel()
        {
            Statuses = new Dictionary<string, double>();
        }

        public ProjectStatusViewModel(Building building)
        {
            BuildingId = building.BuildingID;
            Building = building.Designation;
            AreaCount = building.Areas.Count;
            Statuses = new Dictionary<string, double>();
            foreach (var status in building.Areas.Select(a => a.Status).OrderBy(a => a.ListOrder).Distinct())
            {
                var AreasWithStatusByBuilding = status.Area.Where(a => a.BuildingID == building.BuildingID).Count();
                Statuses.Add(status.Designation, (100 * AreasWithStatusByBuilding) / AreaCount);
            }
        }
    }

    public class ProjectDetailsViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        public string Project { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        [Display(Name = "Job Code")]
        [DisplayFormat(NullDisplayText = "-")]
        public string PurchaseOrder { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string Address { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string City { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string State { get; set; }

        [Display(Name = "Postal Code")]
        [DisplayFormat(NullDisplayText = "-")]
        public int? PostalCode { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedTo { get; private set; }

        public int BuildingCount { get; set; }

        public int AreaCount { get; set; }

        public IList<ProjectStatusViewModel> Buildings { get; set; }

        //Constructors

        public ProjectDetailsViewModel(Guid projectId)
        {
            var project = db.Projects.Include(p => p.Buildings)
                .Include(p => p.User.Contact)
                .Single(p => p.ProjectID == projectId);
            ProjectId = project.ProjectID;
            Project = project.Designation;
            PurchaseOrder = project.PurchaseOrder;
            Description = project.Description;
            Address = project.Address;
            City = project.City;
            State = project.State;
            PostalCode = project.PostalCode;
            AssignedTo = project.User.Contact.FirstName + " " + project.User.Contact.LastName;
            BuildingCount = project.Buildings.Count;
            AreaCount = project.Buildings.Select(b => b.Areas).Count();
            Buildings = new List<ProjectStatusViewModel>();
            foreach (var building in project.Buildings.OrderBy(b => b.Designation))
            {
                Buildings.Add(new ProjectStatusViewModel(building));
            }
        }
    }

    public class ProjectCreateViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        [Required]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        [Display(Name = "Project")]
        public string Designation { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        public string Description { get; set; }

        [Display(Name = "Job Code", ShortName = "PO")]
        public string PurchaseOrder { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Cannot be longer than 100 characters.")]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string City { get; set; }

        [DataType(DataType.Text)]
        [StringLength(2, ErrorMessage = "Cannot be longer than 2 characters.")]
        public string State { get; set; }

        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Please enter a 5 digit code.")]
        [Display(Name = "Postal Code", ShortName = "Zip", Prompt = "Enter postal code")]
        public int? PostalCode { get; set; }

        [Display(Name = "Assigned To")]
        [StringLength(128, ErrorMessage = "128 characters max")]
        public string UserId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

        //Constructors
        public ProjectCreateViewModel()
        {
            string role = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db)).FindByName("isEmployee").Id;
            Users = db.Users.Include(u => u.Contact).Include(u => u.Roles)
                .Where(u => u.Roles.Any(r => r.RoleId == role))
                .OrderBy(u => u.Contact.LastName + u.Contact.FirstName)
                .Select(u => new SelectListItem {
                    Value = u.Id,
                    Text = u.Contact.FirstName + " " + u.Contact.LastName
                });
        }

        //Methods
        public Guid Create()
        {
            var project = new Project {
                ProjectID = Guid.Empty, 
                Designation = Designation, 
                Description = Description, 
                PurchaseOrder = PurchaseOrder, 
                Address = Address, 
                City = City, 
                State = State, 
                PostalCode = PostalCode, 
                UserId = UserId
            };
            db.Projects.Add(project);
            db.SaveChanges();
            return project.ProjectID;
        }
    }

    public class ProjectEditViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "50 characters max")]
        public string Designation { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Job Code")]
        [StringLength(20, ErrorMessage = "20 characters max")]
        public string PurchaseOrder { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "100 characters max")]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "50 characters max")]
        public string City { get; set; }

        [DataType(DataType.Text)]
        [StringLength(2, ErrorMessage = "2 letter abbreviation")]
        public string State { get; set; }

        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Please enter a 5 digit code.")]
        public int? PostalCode { get; set; }

        [Required]
        [Display(Name = "Assigned To")]
        [StringLength(128, ErrorMessage = "128 characters max")]
        public string UserId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

        //Constructors
        public ProjectEditViewModel() { }

        public ProjectEditViewModel(Guid projectId)
        {
            string role = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db)).FindByName("isEmployee").Id;
            Project project = db.Projects.Single(p => p.ProjectID == projectId);
            ProjectId = project.ProjectID;
            Designation = project.Designation;
            Description = project.Description;
            PurchaseOrder = project.PurchaseOrder;
            Address = project.Address;
            City = project.City;
            State = project.State;
            PostalCode = project.PostalCode;
            UserId = project.UserId;
            Users = db.Users.Include(u => u.Contact).Include(u => u.Roles)
                            .Where(u => u.Roles.Any(r => r.RoleId == role))
                            .OrderBy(u => u.Contact.LastName + u.Contact.FirstName)
                            .Select(u => new SelectListItem
                            {
                                Value = u.Id,
                                Text = u.Contact.FirstName + " " + u.Contact.LastName
                            });
        }

        //Methods
        public Guid Edit()
        {
            Project project = db.Projects.Find(ProjectId);
            project.Designation = Designation;
            project.Description = Description;
            project.PurchaseOrder = PurchaseOrder;
            project.Address = Address;
            project.City = City;
            project.State = State;
            project.PostalCode = PostalCode;
            project.UserId = UserId;
            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();
            return ProjectId;
        }
    }
}