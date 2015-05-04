﻿using Cec.Models;
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

    public class BuildingCreateViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        [Display(Name = "Project")]
        public string ProjectDesignation { get; set; }

        [Required()]
        [DataType(DataType.Text)]
        [Display(Name = "Building")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string BuildingDesignation { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        public string Description { get; set; }

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
        [Display(Name = "Postal Code", ShortName = "Zip")]
        public Nullable<int> PostalCode { get; set; }

        //Constructors
        public BuildingCreateViewModel(){ }

        public BuildingCreateViewModel(Guid projectId)
        {
            var project = db.Projects.Find(projectId);
            this.ProjectId = project.ProjectID;
            this.ProjectDesignation = project.Designation;
        }

        //Methods
        public Guid Create()
        {
            var building = new Building()
            {
                ProjectID = this.ProjectId, 
                BuildingID = Guid.Empty, 
                Designation = this.BuildingDesignation, 
                Description = this.Description, 
                Address = this.Address, 
                City = this.City, 
                State = this.State, 
                PostalCode = this.PostalCode
            };
            db.Buildings.Add(building);
            db.SaveChanges();
            return building.BuildingID;
        }
    }

    public class BuildingEditViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        [Display(Name = "Project")]
        public string ProjectDesignation { get; set; }

        public Guid BuildingId { get; set; }

        [Required()]
        [DataType(DataType.Text)]
        [Display(Name = "Building")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string BuildingDesignation { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        public string Description { get; set; }

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
        [Display(Name = "Postal Code", ShortName = "Zip")]
        public Nullable<int> PostalCode { get; set; }

        //Constructors
        public BuildingEditViewModel() { }

        public BuildingEditViewModel(Guid buildingId)
        {
            var building = db.Buildings.Find(buildingId);
            this.ProjectId = building.ProjectID;
            this.ProjectDesignation = building.Project.Designation;
            this.BuildingId = building.BuildingID;
            this.BuildingDesignation = building.Designation;
            this.Description = building.Description;
            this.Address = building.Address;
            this.City = building.City;
            this.State = building.State;
            this.PostalCode = building.PostalCode;
        }

        //Methods
        public Guid Edit()
        {
            var building = new Building()
            {
                ProjectID = this.ProjectId,
                BuildingID = this.BuildingId,
                Designation = this.BuildingDesignation,
                Description = this.Description,
                Address = this.Address,
                City = this.City,
                State = this.State,
                PostalCode = this.PostalCode
            };
            db.Entry(building).State = EntityState.Modified;
            db.SaveChanges();
            return this.BuildingId;
        }
    }

    public class BuildingCopyViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        [Display(Name = "Project")]
        public string ProjectDesignation { get; set; }

        public Guid BuildingId { get; set; }

        [Required()]
        [DataType(DataType.Text)]
        [Display(Name = "Building")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string BuildingDesignation { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        public string Description { get; set; }

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
        [Display(Name = "Postal Code", ShortName = "Zip")]
        public Nullable<int> PostalCode { get; set; }

        //Constructors
        public BuildingCopyViewModel() { }

        public BuildingCopyViewModel(Guid buildingId)
        {
            var building = db.Buildings.Find(buildingId);
            this.ProjectId = building.ProjectID;
            this.ProjectDesignation = building.Project.Designation;
            this.BuildingId = building.BuildingID;
            this.BuildingDesignation = building.Designation += " Copy";
            this.Description = building.Description;
            this.Address = building.Address;
            this.City = building.City;
            this.State = building.State;
            this.PostalCode = building.PostalCode;
        }

        //Methods
        public Guid Copy()
        {
            var building = new Building()
            {
                ProjectID = this.ProjectId,
                BuildingID = Guid.Empty,
                Designation = this.BuildingDesignation,
                Description = this.Description,
                Address = this.Address,
                City = this.City,
                State = this.State,
                PostalCode = this.PostalCode
            };
            db.Buildings.Add(building);
            db.SaveChanges();
            return building.BuildingID;
        }
    }

    public class BuildingDeleteViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        [Display(Name = "Project")]
        public string ProjectDesignation { get; set; }

        public Guid BuildingId { get; set; }

        [Display(Name = "Building")]
        public string BuildingDesignation { get; set; }

        //Constructors
        public BuildingDeleteViewModel(){ }

        public BuildingDeleteViewModel(Guid buildingId)
        {
            var building = db.Buildings.Find(buildingId);
            this.ProjectId = building.ProjectID;
            this.ProjectDesignation = building.Project.Designation;
            this.BuildingId = building.BuildingID;
            this.BuildingDesignation = building.Designation;
        }

        //Methods
        public Guid Delete()
        {
            var building = db.Buildings.Find(this.BuildingId);
            db.Buildings.Remove(building);
            db.SaveChanges();
            return this.ProjectId;
        }
    }

    public class BuildingsMaterialViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public bool Selected { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectDesignation { get; set; }

        public Guid BuildingId { get; set; }

        public string BuildingDesignation { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public Guid MaterialId { get; set; }

        [Display(Name = "Material")]
        public string MaterialDesignation { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }

        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public BuildingsMaterialViewModel()
        {

        }

        public BuildingsMaterialViewModel(IGrouping<Guid, AreaMaterial> item)
        {
            this.ProjectId = item.First().Area.Building.ProjectID;
            this.ProjectDesignation = item.First().Area.Building.Project.Designation;
            this.BuildingId = item.First().Area.BuildingID;
            this.BuildingDesignation = item.First().Area.Building.Designation;
            this.MaterialId = item.First().MaterialID;
            this.MaterialDesignation = item.First().Material.Designation;
            this.ImagePath = item.First().Material.ImagePath;
            this.Total = item.Sum(i => i.Quantity);
            this.UnitOfMeasure = item.First().Material.UnitOfMeasure.Designation;
        }

        //Methods
        public List<BuildingsMaterialViewModel> ListByBuildings(List<BuildingIndexViewModel> buildings)
        {
            if (buildings != null)
            {
                var buildingsMaterialList = new List<BuildingsMaterialViewModel>();
                var areaMaterialList = new List<AreaMaterial>();
                foreach (var building in buildings)
                {
                    areaMaterialList.AddRange(db.AreaMaterials.Include(a => a.Material)
                                                              .Include(a => a.Material.UnitOfMeasure)
                                                              .Where(a => a.Area.BuildingID == building.BuildingId)
                                                              .OrderBy(a => a.Material.Designation));
                }
                var materials = areaMaterialList.GroupBy(m => m.MaterialID);
                foreach (var item in materials)
                {
                    var buildingsMaterial = new BuildingsMaterialViewModel(item);
                    buildingsMaterialList.Add(buildingsMaterial);
                }
                return buildingsMaterialList;
            }
            else
            {
                return null;
            }
        }
    }

    public class BMViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties

        public Guid ProjectId { get; set; }

        public string ProjectDesignation { get; set; }

        public IDictionary<Guid, string > Buildings { get; set; }

        public IList<BuildingMaterial> Materials { get; set; }

        //Constructors
        public BMViewModel()
        {

        }

        public BMViewModel(IGrouping<Guid, AreaMaterial> item)
        {
            this.ProjectId = item.First().Area.Building.ProjectID;
            this.ProjectDesignation = item.First().Area.Building.Project.Designation;
            this.Buildings = new Dictionary<Guid, string>();
            this.Materials = new List<BuildingMaterial>();
        }

        //Methods
        public IList<BuildingMaterial> ListByBuildings(List<BuildingIndexViewModel> buildings)
        {
            if (buildings != null)
            {
                var buildingsMaterialList = new List<BuildingsMaterialViewModel>();
                var areaMaterialList = new List<AreaMaterial>();
                foreach (var building in buildings)
                {
                    areaMaterialList.AddRange(db.AreaMaterials.Include(a => a.Material)
                                                              .Include(a => a.Material.UnitOfMeasure)
                                                              .Where(a => a.Area.BuildingID == building.BuildingId)
                                                              .OrderBy(a => a.Material.Designation));
                    this.Buildings.Add(building.BuildingId, building.Building);
                }
                var materials = areaMaterialList.GroupBy(m => m.MaterialID);
                foreach (var item in materials)
                {
                    var m = new BuildingMaterial()
                    {
                        MaterialId = item.First().MaterialID, 
                        MaterialDesignation = item.First().Material.Designation, 
                        ImagePath = item.First().Material.ImagePath, 
                        Total = item.Sum(i => i.Quantity), 
                        UnitOfMeasure = item.First().Material.UnitOfMeasure.Designation
                    };
                    this.Materials.Add(m);
                }
                return this.Materials;
            }
            else
            {
                return null;
            }
        }
    }

    public class BuildingMaterial
    {
        //Public Properties
        public bool Selected { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public Guid MaterialId { get; set; }

        [Display(Name = "Material")]
        public string MaterialDesignation { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }

        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public BuildingMaterial()
        {

        }
    }

    public class BuildingsMaterialCsvViewModel
    {
        //Public Properties
        public string Project { get; set; }

        public string Material { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }

        [Display(Name = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public BuildingsMaterialCsvViewModel()
        {

        }

        public BuildingsMaterialCsvViewModel(BuildingsMaterialViewModel buildingsMaterial)
        {
            this.Project = buildingsMaterial.ProjectDesignation;
            this.Material = buildingsMaterial.MaterialDesignation;
            this.Total = buildingsMaterial.Total;
            this.UnitOfMeasure = buildingsMaterial.UnitOfMeasure;
        }
    }
}