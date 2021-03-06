﻿using Cec.Helpers;
using Cec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Cec.ViewModels
{
    public class StatusSelectList : SelectList
    {
        //Constructors
        public StatusSelectList()
            : base(items(), "Value", "Text") { }

        public StatusSelectList(object selectedValue)
            : base(items(), "Value", "Text", selectedValue) { }

        //Static Methods
        public static System.Collections.IEnumerable items()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Statuses.OrderBy(s => s.ListOrder)
                              .Select(s => new SelectListItem {
                                  Value = s.StatusId.ToString(),
                                  Text = s.Designation
                              });
        }
    }

    public class AreaIndexViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Propeties
        public Guid ProjectId { get; set; }
        public string Project { get; set; }
        public Guid BuildingId { get; set; }
        public string Building { get; set; }
        public ICollection<AreaIndexItemViewModel> Areas { get; set; }

        //Constructors
        public AreaIndexViewModel()
        {
            this.Areas = new List<AreaIndexItemViewModel>();
        }

        public AreaIndexViewModel(Guid buildingId)
        {
            var buildingData = db.Buildings.Find(buildingId);
            this.ProjectId = buildingData.ProjectID;
            this.Project = buildingData.Project.Designation;
            this.BuildingId = buildingData.BuildingID;
            this.Building = buildingData.Designation;
            this.Areas = new List<AreaIndexItemViewModel>();

            foreach (var area in buildingData.Areas.OrderBy(a => a.Designation))
            {
                this.Areas.Add(new AreaIndexItemViewModel(area.AreaID));
            }
        }
    }

    public class AreaIndexItemViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid AreaId { get; set; }
        public string Area { get; set; }
        public bool Selected { get; set; }

        //Constructors
        public AreaIndexItemViewModel() { }

        public AreaIndexItemViewModel(Guid areaId)
        {
            var areaData = db.Areas.Find(areaId);
            this.AreaId = areaData.AreaID;
            this.Area = areaData.Designation;
        }
    }

    public class AreaDetailsViewModel
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

        public Guid AreaId { get; set; }

        [Display(Name = "Area")]
        public string AreaDesignation { get; set; }

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

        public string Status { get; set; }

        [Display(Name="Status Changed")]
        public DateTime StatusChanged { get; set; }

        public Guid? ModelId { get; set; }

        [Display(Name = "Model")]
        [DisplayFormat(NullDisplayText = "-")]
        public string ModelDesignation { get; set; }

        //Constructors
        public AreaDetailsViewModel()
        {

        }

        public AreaDetailsViewModel(Guid areaId)
        {
            var area = db.Areas.Find(areaId);
            this.Address = area.Address;
            this.AreaDesignation = area.Designation;
            this.AreaId = area.AreaID;
            this.BuildingDesignation = area.Building.Designation;
            this.BuildingId = area.BuildingID;
            this.City = area.City;
            this.Description = area.Description;
            if (area.Model != null)
            {
                this.ModelDesignation = area.Model.Designation; 
            }
            this.ModelId = area.ModelID;
            this.PostalCode = area.PostalCode;
            this.ProjectDesignation = area.Building.Project.Designation;
            this.ProjectId = area.Building.ProjectID;
            this.State = area.State;
            this.Status = area.Status.Designation;
            this.StatusChanged = area.StatusChanged;
        }
    }

    public class AreaCreateViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        [Display(Name = "Project")]
        public string ProjectDesignation { get; set; }

        [Required]
        public Guid BuildingId { get; set; }

        [Display(Name = "Building")]
        public string BuildingDesignation { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Area")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string AreaDesignation { get; set; }

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
        [Display(Name = "Postal Code", ShortName = "Zip", Prompt = "Enter postal code")]
        public Nullable<int> PostalCode { get; set; }

        [Required]
        public Guid StatusId { get; set; }

        public Guid? ModelId { get; set; }

        public ModelSelectList Models { get; set; }

        public StatusSelectList Statuses { get; set; }

        //Constructors
        public AreaCreateViewModel(){ }

        public AreaCreateViewModel(Guid buildingId)
        {
            var building = db.Buildings.Find(buildingId);
            this.ProjectId = building.ProjectID;
            this.ProjectDesignation = building.Project.Designation;
            this.BuildingId = building.BuildingID;
            this.BuildingDesignation = building.Designation;
            this.Address = building.Address;
            this.City = building.City;
            this.State = building.State;
            this.PostalCode = building.PostalCode;
            this.Models = new ModelSelectList(this.ProjectId);
            this.Statuses = new StatusSelectList();
        }

        //Methods
        public Guid Create()
        {
            var area = new Area()
            {
                Address = this.Address, 
                AreaID = Guid.Empty, 
                BuildingID = this.BuildingId, 
                City = this.City, 
                Description = this.Description, 
                Designation = this.AreaDesignation, 
                ModelID = this.ModelId, 
                PostalCode = this.PostalCode, 
                State = this.State, 
                StatusId = this.StatusId, 
                StatusChanged = DateTime.Now
            };
            db.Areas.Add(area);
            db.SaveChanges();
            if (area.ModelID != null)
            {
                var modelMaterials = db.ModelMaterials.Where(p => p.ModelID == area.ModelID);
                foreach (var item in modelMaterials)
                {
                    var areaMaterial = new AreaMaterial()
                    {
                        AreaID = area.AreaID, 
                        MaterialID = item.MaterialID, 
                        Quantity = item.Quantity
                    };
                    db.AreaMaterials.Add(areaMaterial);
                }
                db.SaveChanges();
            }
            return area.AreaID;
        }
    }

    public class AreaEditViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        [Display(Name = "Project")]
        public string ProjectDesignation { get; set; }

        [Required]
        public Guid BuildingId { get; set; }

        [Display(Name = "Building")]
        public string BuildingDesignation { get; set; }

        [Required]
        public Guid AreaId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Area")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string AreaDesignation { get; set; }

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
        [Display(Name = "Postal Code", ShortName = "Zip", Prompt = "Enter postal code")]
        public Nullable<int> PostalCode { get; set; }

        [Required]
        public Guid StatusId { get; set; }

        [Display(Name = "Status Changed")]
        public DateTime StatusChanged { get; set; }

        public Guid? ModelId { get; set; }

        public ModelSelectList Models { get; set; }

        public StatusSelectList Statuses { get; set; }

        //Constructors
        public AreaEditViewModel(){ }

        public AreaEditViewModel(Guid areaId)
        {
            var area = db.Areas.Find(areaId);
            this.Address = area.Address;
            this.AreaDesignation = area.Designation;
            this.AreaId = area.AreaID;
            this.BuildingDesignation = area.Building.Designation;
            this.BuildingId = area.BuildingID;
            this.City = area.City;
            this.Description = area.Description;
            this.ModelId = area.ModelID;
            this.PostalCode = area.PostalCode;
            this.ProjectDesignation = area.Building.Project.Designation;
            this.ProjectId = area.Building.ProjectID;
            this.State = area.State;
            this.StatusId = area.StatusId;
            this.StatusChanged = area.StatusChanged;
            this.Models = new ModelSelectList(this.ProjectId, this.ModelId);
            this.Statuses = new StatusSelectList(this.StatusId);
        }

        //Methods
        public Guid Edit(AreaEditViewModel originalModel)
        {
            var area = new Area()
            {
                Address = this.Address, 
                AreaID = this.AreaId, 
                BuildingID = this.BuildingId, 
                City = this.City, 
                Description = this.Description, 
                Designation = this.AreaDesignation, 
                ModelID = this.ModelId, 
                PostalCode = this.PostalCode, 
                State = this.State, 
                StatusId = this.StatusId,
                StatusChanged = (originalModel.StatusId != this.StatusId) ? DateTime.Now : originalModel.StatusChanged
            };
            db.Entry(area).State = EntityState.Modified;
            if (originalModel.ModelId != this.ModelId)
            {
                var areaMaterials = db.AreaMaterials.Where(am => am.AreaID == this.AreaId);
                foreach (var item in areaMaterials)
                {
                    db.AreaMaterials.Remove(item);
                }
                db.SaveChanges();
                var modelMaterials = db.ModelMaterials.Where(mm => mm.ModelID == this.ModelId);
                foreach (var item in modelMaterials)
                {
                    var areaMaterial = new AreaMaterial()
                    {
                        AreaID = this.AreaId, 
                        MaterialID = item.MaterialID, 
                        Quantity = item.Quantity
                    };
                    db.AreaMaterials.Add(areaMaterial);
                }
            }
            db.SaveChanges();
            return this.AreaId;
        }
    }

    public class AreaCopyViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        [Display(Name = "Project")]
        public string ProjectDesignation { get; set; }

        [Required]
        public Guid BuildingId { get; set; }

        [Display(Name = "Building")]
        public string BuildingDesignation { get; set; }

        [Required]
        public Guid AreaId { get; set; }

        public string OriginalArea { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Area")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string AreaDesignation { get; set; }

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
        [Display(Name = "Postal Code", ShortName = "Zip", Prompt = "Enter postal code")]
        public Nullable<int> PostalCode { get; set; }

        public Guid? ModelId { get; set; }

        public BuildingSelectList Buildings { get; set; }

        //Constructors
        public AreaCopyViewModel(){ }

        public AreaCopyViewModel(Guid areaId)
        {
            var area = db.Areas.Find(areaId);
            this.Address = area.Address;
            this.OriginalArea = area.Designation;
            this.AreaId = area.AreaID;
            this.BuildingDesignation = area.Building.Designation;
            this.BuildingId = area.BuildingID;
            this.City = area.City;
            this.Description = area.Description;
            this.ModelId = area.ModelID;
            this.PostalCode = area.PostalCode;
            this.ProjectDesignation = area.Building.Project.Designation;
            this.ProjectId = area.Building.ProjectID;
            this.State = area.State;
            this.Buildings = new BuildingSelectList(this.ProjectId);
        }

        //Methods
        public Guid Copy(Guid buildingId)
        {
            var area = new Area()
            {
                AreaID = Guid.Empty, 
                Designation = this.AreaDesignation, 
                Description = this.Description, 
                Address = this.Address, 
                City = this.City, 
                State = this.State, 
                PostalCode = this.PostalCode,
                BuildingID = buildingId, 
                ModelID = this.ModelId, 
                StatusId = new DefaultStatus().GetDefault(), 
                StatusChanged = DateTime.Now
            };
            db.Areas.Add(area);
            db.SaveChanges();
            var areaMaterials = db.AreaMaterials.Where(am => am.AreaID == this.AreaId);
            foreach (var item in areaMaterials)
            {
                var areaMaterial = new AreaMaterial()
                {
                    AreaID = area.AreaID,
                    MaterialID = item.MaterialID,
                    Quantity = item.Quantity
                };
                db.AreaMaterials.Add(areaMaterial);
            }
            db.SaveChanges();
            return area.AreaID;
        }
    }

    public class AreaDeleteViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        public string ProjectDesignation { get; set; }

        public Guid BuildingId { get; set; }

        public string BuildingDesignation { get; set; }

        public Guid AreaId { get; set; }

        [Display(Name = "Area")]
        public string AreaDesignation { get; set; }

        //Constructors
        public AreaDeleteViewModel()
        {

        }

        public AreaDeleteViewModel(Guid areaId)
        {
            var area = db.Areas.Find(areaId);
            this.AreaDesignation = area.Designation;
            this.AreaId = area.AreaID;
            this.BuildingDesignation = area.Building.Designation;
            this.BuildingId = area.BuildingID;
            this.ProjectDesignation = area.Building.Project.Designation;
            this.ProjectId = area.Building.ProjectID;
        }

        //Methods
        public Guid Delete()
        {
            var area = db.Areas.Find(this.AreaId);
            db.Areas.Remove(area);
            db.SaveChanges();
            return this.BuildingId;
        }
    }

    public class AreasMaterialViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string Project { get; set; }
        public Guid BuildingId { get; set; }
        public string Building { get; set; }
        public IDictionary<Guid, string> Areas { get; set; }
        public ICollection<AreasMaterialItemViewModel> Materials { get; set; }

        //Constructors
        public AreasMaterialViewModel()
        {
            this.Materials = new List<AreasMaterialItemViewModel>();
        }

        public AreasMaterialViewModel(AreaIndexViewModel aivm)
        {
            this.ProjectId = aivm.ProjectId;
            this.Project = aivm.Project;
            this.BuildingId = aivm.BuildingId;
            this.Building = aivm.Building;
            this.Areas = new Dictionary<Guid, string>();
            this.Materials = new List<AreasMaterialItemViewModel>();

            var areaMaterials = new List<AreaMaterial>();
            foreach (var area in aivm.Areas)
            {
                if (area.Selected)
                {
                    this.Areas.Add(area.AreaId, area.Area);

                    areaMaterials.AddRange(db.AreaMaterials.Include(a => a.Material)
                                                           .Include(a => a.Material.UnitOfMeasure)
                                                           .Where(a => a.AreaID == area.AreaId)
                                                           .OrderBy(a => a.Material.Designation));
                }
            }

            foreach (var material in areaMaterials.GroupBy(m => m.MaterialID))
            {
                this.Materials.Add(new AreasMaterialItemViewModel(material.First(), material.Sum(t => t.Quantity)));
            }
        }
    }

    public class AreasMaterialItemViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public bool Selected { get; set; }
        [Display(Name = "Image")]
        public string ImagePath { get; set; }
        public Guid MaterialId { get; set; }
        public string Material { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }
        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public AreasMaterialItemViewModel() { }

        public AreasMaterialItemViewModel(AreaMaterial areaMaterial, double totalQuantity)
        {
            this.ImagePath = areaMaterial.Material.ImagePath;
            this.MaterialId = areaMaterial.MaterialID;
            this.Material = areaMaterial.Material.Designation;
            this.Total = totalQuantity;
            this.UnitOfMeasure = areaMaterial.Material.UnitOfMeasure.Designation;
        }
    }

    public class AreasMaterialCsvViewModel
    {
        //Public Properties
        public IList<AreasMaterialCsvItemViewModel> Materials { get; set; }

        //Constructors
        public AreasMaterialCsvViewModel()
        {
            this.Materials = new List<AreasMaterialCsvItemViewModel>();
        }

        public AreasMaterialCsvViewModel(AreasMaterialViewModel amvm)
        {
            this.Materials = new List<AreasMaterialCsvItemViewModel>();

            foreach (var material in amvm.Materials.Where(m => m.Selected))
            {
                this.Materials.Add(new AreasMaterialCsvItemViewModel(material));
            }
        }
    }

    public class AreasMaterialCsvItemViewModel
    {
        //Public Properties
        public string Material { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }
        [Display(Name = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public AreasMaterialCsvItemViewModel() { }

        public AreasMaterialCsvItemViewModel(AreasMaterialItemViewModel amivm)
        {
            this.Material = amivm.Material;
            this.Total = amivm.Total;
            this.UnitOfMeasure = amivm.UnitOfMeasure;
        }
    }

    public class AreaStatusViewModel
    {
        public IGrouping<string, Area> areaGroup { get; set; }

        //Constructors
        public AreaStatusViewModel() { }

        public AreaStatusViewModel(IGrouping<string, Area> group)
        {
            this.areaGroup = group;
        }
    }
}

