using Cec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace Cec.ViewModels
{
    public class MaterialIndexViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Propeties
        public Guid MaterialID { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        //Constructors
        public MaterialIndexViewModel()
        {

        }

        //Methods
        public List<MaterialIndexViewModel> GetList(string sortOrder, string searchString)
        {
            var materials = db.Materials.Select(m => m);
            if (!String.IsNullOrEmpty(searchString))
            {
                materials = materials.Where(s => s.Description.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    materials = materials.OrderByDescending(m => m.Designation);
                    break;
                default:
                    materials = materials.OrderBy(m => m.Designation);
                    break;
            }
            var lmivm = new List<MaterialIndexViewModel>();
            if (materials.Count() > 0)
            {
                foreach (var item in materials)
                {
                    var material = new MaterialIndexViewModel()
                    {
                        MaterialID = item.MaterialID,
                        Material = item.Designation,
                        Description = item.Description,
                        ImagePath = item.ImagePath
                    };
                    lmivm.Add(material);
                }
            }
            return lmivm;
        }
    }

    public class MaterialDetailsViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Propeties
        public Guid MaterialId { get; set; }
        public string Material { get; set; }
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        [Display(Name = "U/M", ShortName = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public MaterialDetailsViewModel()
        {

        }

        public MaterialDetailsViewModel(Guid materialId)
        {
            var material = db.Materials.Find(materialId);
            this.Description = material.Description;
            this.ImagePath = material.ImagePath;
            this.Material = material.Designation;
            this.MaterialId = material.MaterialID;
            this.UnitOfMeasure = material.UnitOfMeasure.Designation;
        }
    }

    public class MaterialCreateViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Propeties
        [Required()]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string Material { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        [DisplayFormat(NullDisplayText = "-")]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Unit of measure is required.")]
        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public Guid UnitOfMeasureId { get; set; }

        //Constructors
        public MaterialCreateViewModel()
        {

        }

        //Methods
        public Guid Create()
        {
            var material = new Material();
            material.Description = this.Description;
            material.Designation = this.Material;
            material.ImagePath = this.ImagePath;
            material.MaterialID = Guid.Empty;
            material.UnitOfMeasureID = this.UnitOfMeasureId;
            db.Materials.Add(material);
            db.SaveChanges();
            return material.MaterialID;
        }
    }

    public class MaterialEditViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Propeties
        public Guid MaterialId { get; set; }

        [Required()]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string Material { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        [DisplayFormat(NullDisplayText = "-")]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Unit of measure is required.")]
        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public Guid UnitOfMeasureId { get; set; }

        //Constructors
        public MaterialEditViewModel()
        {

        }

        public MaterialEditViewModel(Guid materialId)
        {
            var material = db.Materials.Find(materialId);
            this.Description = material.Description;
            this.ImagePath = material.ImagePath;
            this.Material = material.Designation;
            this.MaterialId = material.MaterialID;
            this.UnitOfMeasureId = material.UnitOfMeasureID ?? Guid.Empty;
        }

        //Methods
        public Guid Edit()
        {
            var material = new Material();
            material.Description = this.Description;
            material.Designation = this.Material;
            material.ImagePath = this.ImagePath;
            material.MaterialID = this.MaterialId;
            material.UnitOfMeasureID = this.UnitOfMeasureId;
            db.Entry(material).State = EntityState.Modified;
            db.SaveChanges();
            return this.MaterialId;
        }
    }

    public class MaterialCopyViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Propeties
        public Guid MaterialId { get; set; }

        [Required()]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string Material { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image")]
        [DisplayFormat(NullDisplayText = "-")]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Unit of measure is required.")]
        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public Guid UnitOfMeasureId { get; set; }

        //Constructors
        public MaterialCopyViewModel()
        {

        }

        public MaterialCopyViewModel(Guid materialId)
        {
            var material = db.Materials.Find(materialId);
            this.Description = material.Description;
            this.ImagePath = material.ImagePath;
            this.Material = material.Designation += " Copy";
            this.MaterialId = material.MaterialID;
            this.UnitOfMeasureId = material.UnitOfMeasureID ?? Guid.Empty;
        }

        //Methods
        public Guid Copy()
        {
            var material = new Material();
            material.Description = this.Description;
            material.Designation = this.Material;
            material.ImagePath = this.ImagePath;
            material.MaterialID = Guid.Empty;
            material.UnitOfMeasureID = this.UnitOfMeasureId;
            db.Materials.Add(material);
            db.SaveChanges();
            return material.MaterialID;
        }
    }
}