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
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Buildings.Where(b => b.ProjectID == projectId)
                               .OrderBy(b => b.Designation)
                               .Select(b => new SelectListItem {
                                   Value = b.BuildingID.ToString(),
                                   Text = b.Designation
                               });
        }
    }

    public class BuildingIndexViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string ProjectDesignation { get; set; }
        public ICollection<BuildingIndexItemViewModel> Buildings { get; set; }

        //Constructors
        public BuildingIndexViewModel()
        {
            this.Buildings = new List<BuildingIndexItemViewModel>();
        }

        public BuildingIndexViewModel(Guid projectId)
        {
            var projectData = db.Projects.Find(projectId);
            this.ProjectId = projectData.ProjectID;
            this.ProjectDesignation = projectData.Designation;
            this.Buildings = new List<BuildingIndexItemViewModel>();

            foreach (var building in projectData.Buildings.OrderBy(b => b.Designation))
            {
                this.Buildings.Add(new BuildingIndexItemViewModel(building.BuildingID));
            }
        }
    }

    public class BuildingIndexItemViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid BuildingId { get; set; }

        public string Building { get; set; }

        public bool Selected { get; set; }

        //Constructors
        public BuildingIndexItemViewModel() { }

        public BuildingIndexItemViewModel(Guid buildingId)
        {
            var buildingData = db.Buildings.Find(buildingId);
            BuildingId = buildingData.BuildingID;
            Building = buildingData.Designation;
        }
    }

    public class BuildingStatusViewModel
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Guid StatusId { get; set; }

        public string Status { get; set; }

        public int AreaCount { get; set; }

        public double Percent { get; set; }

        public ICollection<AreaStatusViewModel> AreaStatuses { get; set; }

        public BuildingStatusViewModel()
        {
            AreaStatuses = new List<AreaStatusViewModel>();
        }

        public BuildingStatusViewModel(Status status, Guid buildingId)
        {
            var building = db.Buildings.Find(buildingId);
            StatusId = status.StatusId;
            Status = status.Designation;
            AreaCount = status.Area.Where(a => a.BuildingID == buildingId).Count();
            Percent = (100 * AreaCount) / building.Areas.Count;
            AreaStatuses = new List<AreaStatusViewModel>();
            foreach (var item in building.Areas.Where(a => a.StatusId == this.StatusId).OrderBy(a => a.StatusChanged).GroupBy(a => a.StatusChanged.ToShortDateString()))
            {
                AreaStatuses.Add(new AreaStatusViewModel(item));
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
            foreach (var status in building.Areas.Select(a => a.Status).Distinct().OrderBy(s => s.ListOrder))
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
            this.Address = project.Address;
            this.City = project.City;
            this.State = project.State;
            this.PostalCode = project.PostalCode;
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

        public string OriginalBuilding { get; set; }

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
            this.OriginalBuilding = building.Designation;
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

        public Guid Copy(Guid includeAreasFromOriginal)
        {
            var buildingId = this.Copy();
            var areas = db.Buildings.Find(includeAreasFromOriginal).Areas;
            foreach (var item in areas)
            {
                var a = new AreaCopyViewModel(item.AreaID).Copy(buildingId);
            }
            return buildingId;
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
        public Guid ProjectId { get; set; }
        public string Project { get; set; }
        public IDictionary<Guid, string> Buildings { get; set; }
        public ICollection<BuildingsMaterialItemViewModel> Materials { get; set; }

        //Constructors
        public BuildingsMaterialViewModel()
        {
            this.Materials = new List<BuildingsMaterialItemViewModel>();
        }

        public BuildingsMaterialViewModel(BuildingIndexViewModel bivm)
        {
            this.ProjectId = bivm.ProjectId;
            this.Project = bivm.ProjectDesignation;
            this.Buildings = new Dictionary<Guid, string>();
            this.Materials = new List<BuildingsMaterialItemViewModel>();

            var buildingsMaterial = new List<AreaMaterial>();
            foreach (var building in bivm.Buildings)
            {
                if (building.Selected)
                {
                    this.Buildings.Add(building.BuildingId, building.Building);

                    buildingsMaterial.AddRange(db.AreaMaterials.Include(a => a.Material)
                                                               .Include(a => a.Material.UnitOfMeasure)
                                                               .Where(a => a.Area.BuildingID == building.BuildingId)
                                                               .OrderBy(a => a.Material.Designation));
                }
            }

            foreach (var material in buildingsMaterial.GroupBy(m => m.MaterialID))
            {
                this.Materials.Add(new BuildingsMaterialItemViewModel(material.First(), material.Sum(t => t.Quantity)));
            }
        }
    }

    public class BuildingsMaterialItemViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public bool Selected { get; set; }
        [Display(Name = "Image")]
        public string ImagePath { get; set; }
        public Guid MaterialId { get; set; }
        [Display(Name = "Material")]
        public string Material { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }
        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public BuildingsMaterialItemViewModel() { }

        public BuildingsMaterialItemViewModel(AreaMaterial areaMaterial, double totalQuantity)
        {
            this.ImagePath = areaMaterial.Material.ImagePath;
            this.MaterialId = areaMaterial.MaterialID;
            this.Material = areaMaterial.Material.Designation;
            this.Total = totalQuantity;
            this.UnitOfMeasure = areaMaterial.Material.UnitOfMeasure.Designation;
        }
    }

    public class BuildingsMaterialCsvViewModel
    {
        //Public Properties
        public string Project { get; set; }
        public IList<BuildingsMaterialCsvItemViewModel> Materials { get; set; }

        //Constructors
        public BuildingsMaterialCsvViewModel()
        {
            this.Materials = new List<BuildingsMaterialCsvItemViewModel>();
        }

        public BuildingsMaterialCsvViewModel(BuildingsMaterialViewModel bmvm)
        {
            this.Project = bmvm.Project;
            this.Materials = new List<BuildingsMaterialCsvItemViewModel>();

            foreach (var material in bmvm.Materials.Where(m => m.Selected))
            {
                this.Materials.Add(new BuildingsMaterialCsvItemViewModel(material));
            }
        }
    }

    public class BuildingsMaterialCsvItemViewModel
    {
        //Public Properties
        public string Material { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }
        [Display(Name = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public BuildingsMaterialCsvItemViewModel() { }

        public BuildingsMaterialCsvItemViewModel(BuildingsMaterialItemViewModel bmivm)
        {
            this.Material = bmivm.Material;
            this.Total = bmivm.Total;
            this.UnitOfMeasure = bmivm.UnitOfMeasure;
        }
    }
}