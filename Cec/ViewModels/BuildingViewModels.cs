using Cec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Cec.ViewModels
{
    public class BuildingSelectList : SelectList
    {
        //Constructors
        public BuildingSelectList(Guid projectId)
            : base(items(projectId), "Value", "Text") { }

        public BuildingSelectList(Guid projectId, object selectedValue)
            : base(items(projectId), "Value", "Text", selectedValue) { }

        //Static Metods
        public static System.Collections.IEnumerable items(Guid projectId)
        {
            var db = new ApplicationDbContext();
            var selectListItems = new List<SelectListItem>();
            var buildings = db.Buildings.Where(b => b.ProjectID == projectId)
                                        .OrderBy(b => b.Designation);
            foreach (var item in buildings)
            {
                var building = new SelectListItem()
                {
                    Value = item.BuildingID.ToString(),
                    Text = item.Designation
                };
                selectListItems.Add(building);
            }
            return selectListItems;
        }
    }

    public class BuildingIndexViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string Project { get; set; }
        public Guid BuildingId { get; set; }
        public string Building { get; set; }
        public bool Selected { get; set; }

        //Constructors
        public BuildingIndexViewModel()
        { 
            
        }

        public BuildingIndexViewModel(Building building)
        {
            this.ProjectId = building.ProjectID;
            this.Project = building.Project.Designation;
            this.BuildingId = building.BuildingID;
            this.Building = building.Designation;
        }

        //Methods
        public List<BuildingIndexViewModel> ListByProject(Guid projectId)
        {
            var buildings = db.Buildings.Include(b => b.Project)
                                        .Where(b => b.ProjectID == projectId)
                                        .OrderBy(b => b.Designation);
            if (buildings.Count() > 0)
            {
                var buildingIndexViewModels = new List<BuildingIndexViewModel>();
                foreach (var building in buildings)
                {
                    var buildingIndexViewModel = new BuildingIndexViewModel(building);
                    buildingIndexViewModels.Add(buildingIndexViewModel);
                }
                return buildingIndexViewModels;
            }
            else
            {
                return null;
            }
        }
    }

    public class BuildingStatusViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Guid StatusId { get; set; }
        public string Status { get; set; }
        public int AreaCount { get; set; }
        public double Percent { get; set; }
        public IDictionary<Guid, string> Areas { get; set; }

        public BuildingStatusViewModel()
        {
            this.Areas = new Dictionary<Guid, string>();
        }

        public BuildingStatusViewModel(Status status, Guid buildingId)
        {
            var building = db.Buildings.Find(buildingId);
            this.StatusId = status.StatusId;
            this.Status = status.Designation;
            this.AreaCount = status.Area.Where(a => a.BuildingID == buildingId).Count();
            this.Percent = (100 * this.AreaCount) / building.Areas.Count;
            this.Areas = new Dictionary<Guid, string>();
            foreach (var area in building.Areas.Where(a => a.StatusId == this.StatusId))
            {
                this.Areas.Add(area.AreaID, area.Designation);
            }
        }
    }

    public class BuildingDetailsViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        [Display(Name = "Project")]
        public string Project { get; set; }

        public Guid BuildingId { get; set; }

        [Display(Name = "Building")]
        public string Building { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string Address { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string City { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public string State { get; set; }

        [Display(Name = "Postal Code", ShortName = "Zip")]
        [DisplayFormat(NullDisplayText = "-")]
        public Nullable<int> PostalCode { get; set; }

        public int AreaCount { get; set; }

        public IList<BuildingStatusViewModel> Statuses { get; set; }

        //Constructors
        public BuildingDetailsViewModel()
        {

        }

        public BuildingDetailsViewModel(Guid buildingId)
        {
            var building = db.Buildings.Find(buildingId);
            this.ProjectId = building.ProjectID;
            this.Project = building.Project.Designation;
            this.BuildingId = building.BuildingID;
            this.Building = building.Designation;
            this.Description = building.Description;
            this.Address = building.Address;
            this.City = building.City;
            this.State = building.State;
            this.PostalCode = building.PostalCode;
            this.AreaCount = building.Areas.Count;
            this.Statuses = new List<BuildingStatusViewModel>();
            foreach (var status in building.Areas.Select(a => a.Status).Distinct().OrderBy(s => s.Designation))
            {
                this.Statuses.Add(new BuildingStatusViewModel(status, this.BuildingId));
            }
        }
    }

    public class BuildingsMaterialViewModel
    {
        //Public Properties
        public bool Selected { get; set; }
        public Guid ProjectId { get; set; }
        public string Project { get; set; }
        public Guid BuildingId { get; set; }
        public string Building { get; set; }
        public string ImagePath { get; set; }
        public Guid MaterialId { get; set; }
        public string Material { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }
        [Display(Name = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public BuildingsMaterialViewModel()
        {

        }

        public BuildingsMaterialViewModel(BuildingIndexViewModel buildingIndexViewModel)
        {
            this.ProjectId = buildingIndexViewModel.ProjectId;
            this.Project = buildingIndexViewModel.Project;
            this.BuildingId = buildingIndexViewModel.BuildingId;
            this.Building = buildingIndexViewModel.Building;
        }
    }

    public class BuildingsMaterialCsvViewModel
    {
        //Public Properties
        public string Project { get; set; }
        public string Material { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }
        [Display(Name = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public BuildingsMaterialCsvViewModel()
        {

        }

        public BuildingsMaterialCsvViewModel(BuildingsMaterialViewModel buildingsMaterialViewModel)
        {
            this.Project = buildingsMaterialViewModel.Project;
            this.Material = buildingsMaterialViewModel.Material;
            this.Total = buildingsMaterialViewModel.Total;
            this.UnitOfMeasure = buildingsMaterialViewModel.UnitOfMeasure;
        }
    }
}