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
            var db = new ApplicationDbContext();
            var selectListItems = new List<SelectListItem>();
            var models = db.Statuses.OrderBy(s => s.Designation);
            foreach (var item in models)
            {
                var model = new SelectListItem()
                {
                    Value = item.StatusId.ToString(),
                    Text = item.Designation
                };
                selectListItems.Add(model);
            }
            return selectListItems;
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
        public Guid AreaId { get; set; }
        public string Area { get; set; }
        public bool Selected { get; set; }


        //Constructors
        public AreaIndexViewModel()
        {

        }

        public AreaIndexViewModel(Area area)
        {
            this.ProjectId = area.Building.ProjectID;
            this.Project = area.Building.Project.Designation;
            this.BuildingId = area.BuildingID;
            this.Building = area.Building.Designation;
            this.AreaId = area.AreaID;
            this.Area = area.Designation;
        }

        //Methods
        public List<AreaIndexViewModel> ListByBuilding(Guid buildingId)
        {
            var areas = db.Areas.Where(a => a.BuildingID == buildingId)
                                .OrderBy(a => a.Designation)
                                .ToList();
            if (areas.Count() > 0)
            {
                var areaIndexViewModels = new List<AreaIndexViewModel>();
                foreach (var item in areas)
                {
                    var areaIndexViewModel = new AreaIndexViewModel(item);
                    areaIndexViewModels.Add(areaIndexViewModel);
                }
                return areaIndexViewModels;
            }
            else
            {
                return null;
            }
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
        public AreaCreateViewModel()
        {

        }

        public AreaCreateViewModel(Guid buildingId)
        {
            var building = db.Buildings.Find(buildingId);
            this.BuildingDesignation = building.Designation;
            this.BuildingId = building.BuildingID;
            this.ProjectDesignation = building.Project.Designation;
            this.ProjectId = building.ProjectID;
            this.Models = new ModelSelectList(this.ProjectId);
            this.Statuses = new StatusSelectList();
        }

        //Methods
        public Guid Create()
        {
            var area = new Area();
            area.Address = this.Address;
            area.AreaID = Guid.Empty;
            area.BuildingID = this.BuildingId;
            area.City = this.City;
            area.Description = this.Description;
            area.Designation = this.AreaDesignation;
            area.ModelID = this.ModelId;
            area.PostalCode = this.PostalCode;
            area.State = this.State;
            area.StatusId = this.StatusId;
            db.Areas.Add(area);
            db.SaveChanges();
            if (area.ModelID != null)
            {
                var modelMaterials = db.ModelMaterials.Where(p => p.ModelID == area.ModelID);
                foreach (var item in modelMaterials)
                {
                    var areaMaterial = new AreaMaterial();
                    areaMaterial.AreaID = area.AreaID;
                    areaMaterial.MaterialID = item.MaterialID;
                    areaMaterial.Quantity = item.Quantity;
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

        public Guid? ModelId { get; set; }

        public ModelSelectList Models { get; set; }

        public StatusSelectList Statuses { get; set; }

        //Constructors
        public AreaEditViewModel()
        {

        }

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
            this.Models = new ModelSelectList(this.ProjectId, this.ModelId);
            this.Statuses = new StatusSelectList(this.StatusId);
        }

        //Methods
        public Guid Edit(Guid? originalModel)
        {
            var area = new Area();
            area.Address = this.Address;
            area.AreaID = this.AreaId;
            area.BuildingID = this.BuildingId;
            area.City = this.City;
            area.Description = this.Description;
            area.Designation = this.AreaDesignation;
            area.ModelID = this.ModelId;
            area.PostalCode = this.PostalCode;
            area.State = this.State;
            area.StatusId = this.StatusId;
            db.Entry(area).State = EntityState.Modified;
            if (originalModel != this.ModelId)
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
                    var areaMaterial = new AreaMaterial();
                    areaMaterial.AreaID = this.AreaId;
                    areaMaterial.MaterialID = item.MaterialID;
                    areaMaterial.Quantity = item.Quantity;
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

        public BuildingSelectList Buildings { get; set; }

        public StatusSelectList Statuses { get; set; }

        //Constructors
        public AreaCopyViewModel()
        {

        }

        public AreaCopyViewModel(Guid areaId)
        {
            var area = db.Areas.Find(areaId);
            this.Address = area.Address;
            this.AreaDesignation = area.Designation += " Copy";
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
            this.Buildings = new BuildingSelectList(this.ProjectId);
            this.Statuses = new StatusSelectList(this.StatusId);
        }

        //Methods
        public Guid Copy()
        {
            var area = new Area();
            area.Address = this.Address;
            area.AreaID = Guid.Empty;
            area.BuildingID = this.BuildingId;
            area.City = this.City;
            area.Description = this.Description;
            area.Designation = this.AreaDesignation;
            area.ModelID = this.ModelId;
            area.PostalCode = this.PostalCode;
            area.State = this.State;
            area.StatusId = this.StatusId;
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
        public bool Selected { get; set; }
        public Guid ProjectId { get; set; }
        public string Project { get; set; }
        public Guid BuildingId { get; set; }
        public string Building { get; set; }
        public Guid AreaId { get; set; }
        public string Area { get; set; }
        public string ImagePath { get; set; }
        public Guid MaterialId { get; set; }
        public string Material { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }
        [Display(Name = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public AreasMaterialViewModel()
        {

        }

        public AreasMaterialViewModel(AreaIndexViewModel areaIndexViewModel)
        {
            this.ProjectId = areaIndexViewModel.ProjectId;
            this.Project = areaIndexViewModel.Project;
            this.BuildingId = areaIndexViewModel.BuildingId;
            this.Building = areaIndexViewModel.Building;
            this.AreaId = areaIndexViewModel.AreaId;
            this.Area = areaIndexViewModel.Area;
        }

        //Methods
        public List<AreasMaterialViewModel> ListByAreas(List<AreaIndexViewModel> areas)
        {
            var areasMaterialViewModels = new List<AreasMaterialViewModel>();
            var areaMaterials = new List<AreaMaterial>();
            foreach (var area in areas)
            {
                areaMaterials.AddRange(db.AreaMaterials.Include(a => a.Material)
                                                       .Include(a => a.Material.UnitOfMeasure)
                                                       .Where(a => a.AreaID == area.AreaId)
                                                       .OrderBy(a => a.Material.Designation));
            }
            var materials = areaMaterials.GroupBy(m => m.MaterialID);
            foreach (var material in materials)
            {
                var areasMaterialViewModel = new AreasMaterialViewModel()
                {
                    ProjectId = material.First().Area.Building.ProjectID,
                    Project = material.First().Area.Building.Project.Designation,
                    BuildingId = material.First().Area.BuildingID,
                    Building = material.First().Area.Building.Designation,
                    AreaId = material.First().AreaID,
                    Area = material.First().Area.Designation,
                    MaterialId = material.First().MaterialID,
                    Material = material.First().Material.Designation,
                    ImagePath = material.First().Material.ImagePath,
                    UnitOfMeasure = material.First().Material.UnitOfMeasure.Designation,
                    Total = material.Sum(i => i.Quantity)
                };
                areasMaterialViewModels.Add(areasMaterialViewModel);
            }
            return areasMaterialViewModels;
        }
    }

    public class AreasMaterialCsvViewModel
    {
        //Public Properties
        public string Areas { get; set; }
        public string Material { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }
        [Display(Name = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public AreasMaterialCsvViewModel()
        {

        }

        public AreasMaterialCsvViewModel(AreasMaterialViewModel areasMaterialViewModel)
        {
            this.Material = areasMaterialViewModel.Material;
            this.Total = areasMaterialViewModel.Total;
            this.UnitOfMeasure = areasMaterialViewModel.UnitOfMeasure;
        }
    }
}
