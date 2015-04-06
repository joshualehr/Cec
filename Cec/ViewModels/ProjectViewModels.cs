using Cec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        public ProjectIndexViewModel()
        { 
            
        }

        //Methods
        public List<ProjectIndexViewModel> ListAll()
        {
            var projects = db.Projects.OrderBy(p => p.Designation);
            if (projects.Count() > 0)
            {
                var projectIndexViewModels = new List<ProjectIndexViewModel>();
                foreach (var project in projects)
                {
                    projectIndexViewModels.Add(new ProjectIndexViewModel() { ProjectId = project.ProjectID, Project = project.Designation });
                }
                return projectIndexViewModels;
            }
            else
            {
                return null;
            }
        }

        public List<ProjectIndexViewModel> ListByUser(string userId)
        {
            var projects = db.Projects.Where(p => p.User.Id == userId).OrderBy(p => p.Designation);
            if (projects.Count() > 0)
            {
                var projectIndexViewModels = new List<ProjectIndexViewModel>();
                foreach (var project in projects)
                {
                    projectIndexViewModels.Add(new ProjectIndexViewModel() { ProjectId = project.ProjectID, Project = project.Designation });
                }
                return projectIndexViewModels;
            }
            else
            {
                return null;
            }
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
            this.Statuses = new Dictionary<string, double>();
        }

        public ProjectStatusViewModel(Building building)
        {
            this.BuildingId = building.BuildingID;
            this.Building = building.Designation;
            this.AreaCount = building.Areas.Count;
            this.Statuses = new Dictionary<string, double>();
            foreach (var status in building.Areas.Select(a => a.Status).Distinct())
            {
                var AreasWithStatusByBuilding = status.Area.Where(a => a.BuildingID == building.BuildingID).Count();
                this.Statuses.Add(status.Designation, (100 * AreasWithStatusByBuilding) / this.AreaCount);
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

        [Display(Name = "Purchase Order", ShortName = "PO")]
        [DisplayFormat(NullDisplayText = "-")]
        public string PurchaseOrder { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string Address { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string City { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string State { get; set; }

        [Display(Name = "Postal Code", ShortName = "Zip")]
        [DisplayFormat(NullDisplayText = "-")]
        public Nullable<int> PostalCode { get; set; }

        public int BuildingCount { get; set; }

        public int AreaCount { get; set; }

        public IList<ProjectStatusViewModel> Buildings { get; set; }

        //Constructors

        public ProjectDetailsViewModel(Guid projectId)
        {
            var project = db.Projects.Find(projectId);
            this.ProjectId = project.ProjectID;
            this.Project = project.Designation;
            this.PurchaseOrder = project.PurchaseOrder;
            this.Description = project.Description;
            this.Address = project.Address;
            this.City = project.City;
            this.State = project.State;
            this.PostalCode = project.PostalCode;
            this.BuildingCount = project.Buildings.Count;
            this.AreaCount = project.Buildings.Select(b => b.Areas).Count();
            this.Buildings = new List<ProjectStatusViewModel>();
            foreach (var building in project.Buildings.OrderBy(b => b.Designation))
            {
                this.Buildings.Add(new ProjectStatusViewModel(building));
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

        [Display(Name = "Purchase Order", ShortName = "PO")]
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
        public Nullable<int> PostalCode { get; set; }

        //Constructors
        public ProjectCreateViewModel()
        {
            
        }

        //Methods
        public Guid Create(string userId)
        {
            var project = new Project();
            project.ProjectID = Guid.Empty;
            project.Designation = this.Designation;
            project.Description = this.Description;
            project.PurchaseOrder = this.PurchaseOrder;
            project.Address = this.Address;
            project.City = this.City;
            project.State = this.State;
            project.PostalCode = this.PostalCode;
            project.UserId = userId;
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
        public string Designation { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        public string Description { get; set; }

        [Display(Name = "Purchase Order", ShortName = "PO")]
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
        public Nullable<int> PostalCode { get; set; }

        //Constructors
        public ProjectEditViewModel()
        {

        }

        public ProjectEditViewModel(Guid projectId)
        {
            var project = db.Projects.Find(projectId);
            this.ProjectId = project.ProjectID;
            this.Designation = project.Designation;
            this.Description = project.Description;
            this.PurchaseOrder = project.PurchaseOrder;
            this.Address = project.Address;
            this.City = project.City;
            this.State = project.State;
            this.PostalCode = project.PostalCode;
        }

        //Methods
        public Guid Edit()
        {
            var project = db.Projects.Find(this.ProjectId);
            project.Designation = this.Designation;
            project.Description = this.Description;
            project.PurchaseOrder = this.PurchaseOrder;
            project.Address = this.Address;
            project.City = this.City;
            project.State = this.State;
            project.PostalCode = this.PostalCode;
            db.Entry(project).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return this.ProjectId;
        }
    }

    public class ProjectDeleteViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        public string Designation { get; set; }

        //Constructors
        public ProjectDeleteViewModel()
        {

        }

        public ProjectDeleteViewModel(Guid projectId)
        {
            var project = db.Projects.Find(projectId);
            this.ProjectId = project.ProjectID;
            this.Designation = project.Designation;
        }

        //Methods
        public Guid Delete()
        {
            var project = db.Projects.Find(this.ProjectId);
            db.Projects.Remove(project);
            db.SaveChanges();
            return project.ProjectID;
        }
    }
}