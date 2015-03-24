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
        public ProjectDetailsViewModel()
        {

        }

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
}