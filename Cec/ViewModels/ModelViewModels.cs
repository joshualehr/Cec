using Cec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Cec.ViewModels
{
    public class ModelSelectList : SelectList
    {
        //Constructors
        public ModelSelectList(Guid projectId)
            : base(items(projectId), "Value", "Text") { }

        public ModelSelectList(Guid projectId, object selectedValue)
            : base(items(projectId), "Value", "Text", selectedValue) { }

        //Static Methods
        public static System.Collections.IEnumerable items(Guid projectId)
        {
            var db = new ApplicationDbContext();
            var selectListItems = new List<SelectListItem>();
            var models = db.Models.Where(p => p.ProjectID == projectId)
                                  .OrderBy(p => p.Designation);
            foreach (var item in models)
            {
                var model = new SelectListItem()
                {
                    Value = item.ModelID.ToString(),
                    Text = item.Designation
                };
                selectListItems.Add(model);
            }
            return selectListItems;
        }
    }

    public class ModelIndexViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<ModelIndexItemViewModel> Models { get; set; }

        //Constructors
        public ModelIndexViewModel() { }

        public ModelIndexViewModel(Guid projectId)
        {
            var projectData = db.Projects.Find(projectId);
            this.ProjectId = projectData.ProjectID;
            this.ProjectName = projectData.Designation;
            this.Models = new List<ModelIndexItemViewModel>();
            var modelsData = db.Models.Where(m => m.ProjectID == projectId)
                                      .OrderBy(m => m.Designation);
            foreach (var item in modelsData)
            {
                this.Models.Add(new ModelIndexItemViewModel(item.ModelID));
            }
        }
    }

    public class ModelIndexItemViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ModelId { get; set; }
        public string ModelName { get; set; }

        //Constructors
        public ModelIndexItemViewModel() { }

        public ModelIndexItemViewModel(Guid modelId)
        {
            var modelData = db.Models.Find(modelId);
            this.ModelId = modelData.ModelID;
            this.ModelName = modelData.Designation;
        }
    }

    public class ModelCreateViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        [Required()]
        [DataType(DataType.Text)]
        [Display(Name = "Model")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string ModelName { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        //Constructors
        public ModelCreateViewModel() { }

        public ModelCreateViewModel(Guid projectId)
        {
            var projectData = db.Projects.Find(projectId);
            this.ProjectId = projectData.ProjectID;
            this.ProjectName = projectData.Designation;
        }

        //Methods
        public Guid Create()
        {
            var model = new Model();
            model.ModelID = Guid.Empty;
            model.Designation = this.ModelName;
            model.Description = this.Description;
            model.ProjectID = this.ProjectId;
            db.Models.Add(model);
            db.SaveChanges();
            return model.ModelID;
        }
    }

    public class ModelDetailsViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        [Display(Name = "Project")]
        public string ProjectName { get; set; }
        public Guid ModelId { get; set; }
        [Display(Name = "Model")]
        public string ModelName { get; set; }
        [DataType(DataType.MultilineText), Display(ShortName = "Desc."), DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        //Constructors
        public ModelDetailsViewModel() { }

        public ModelDetailsViewModel(Guid modelId)
        {
            var modelData = db.Models.Find(modelId);
            this.ProjectId = modelData.ProjectID;
            this.ProjectName = modelData.Project.Designation;
            this.ModelId = modelData.ModelID;
            this.ModelName = modelData.Designation;
            this.Description = modelData.Description;
        }
    }

    public class ModelEditViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid ModelId { get; set; }
        [Required()]
        [DataType(DataType.Text)]
        [Display(Name = "Model")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string ModelName { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        //Constructors
        public ModelEditViewModel() { }

        public ModelEditViewModel(Guid modelId)
        {
            var modelData = db.Models.Find(modelId);
            this.ProjectId = modelData.ProjectID;
            this.ProjectName = modelData.Project.Designation;
            this.ModelId = modelData.ModelID;
            this.ModelName = modelData.Designation;
            this.Description = modelData.Description;
        }

        //Methods
        public Guid Edit()
        {
            var model = db.Models.Find(this.ModelId);
            model.Designation = this.ModelName;
            model.Description = this.Description;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return this.ModelId;
        }
    }

    public class ModelCopyViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid ModelId { get; set; }
        [Required()]
        [DataType(DataType.Text)]
        [Display(Name = "Model")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string ModelName { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        //Constructors
        public ModelCopyViewModel() { }

        public ModelCopyViewModel(Guid modelId)
        {
            var modelData = db.Models.Find(modelId);
            this.ProjectId = modelData.ProjectID;
            this.ProjectName = modelData.Project.Designation;
            this.ModelId = modelData.ModelID;
            this.ModelName = modelData.Designation + " Copy";
            this.Description = modelData.Description;
        }

        //Methods
        public Guid Copy()
        {
            var model = new Model()
            {
                Designation = this.ModelName,
                Description = this.Description,
                ProjectID = this.ProjectId
            };
            db.Models.Add(model);
            db.SaveChanges();
            var modelMaterialsData = db.ModelMaterials.Where(mm => mm.ModelID == this.ModelId);
            foreach (var item in modelMaterialsData)
            {
                var modelMaterial = new ModelMaterial()
                {
                    ModelID = model.ModelID,
                    MaterialID = item.MaterialID,
                    Quantity = item.Quantity
                };
                db.ModelMaterials.Add(modelMaterial);
            }
            db.SaveChanges();
            return model.ModelID;
        }
    }

    public class ModelDeleteViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid ModelId { get; set; }
        public string ModelName { get; set; }

        //Constructors
        public ModelDeleteViewModel() { }

        public ModelDeleteViewModel(Guid modelId)
        {
            var modelData = db.Models.Find(modelId);
            this.ProjectId = modelData.ProjectID;
            this.ProjectName = modelData.Project.Designation;
            this.ModelId = modelData.ModelID;
            this.ModelName = modelData.Designation;
        }

        //Methods
        public Guid Delete()
        {
            var modelData = db.Models.Find(this.ModelId);
            db.Models.Remove(modelData);
            db.SaveChanges();
            return this.ProjectId;
        }
    }

    public class ModelMaterialCreateViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid ModelId { get; set; }
        public string ModelName { get; set; }
        public bool OnlyProjectMaterial { get; set; }
        public bool ApplyToAllAreas { get; set; }
        public List<ModelMaterialCreateItemViewModel> Materials { get; set; }

        //Constructors
        public ModelMaterialCreateViewModel() { }

        public ModelMaterialCreateViewModel(Guid modelId)
        {
            var modelData = db.Models.Find(modelId);
            this.ProjectId = modelData.ProjectID;
            this.ProjectName = modelData.Project.Designation;
            this.ModelId = modelData.ModelID;
            this.ModelName = modelData.Designation;
            this.OnlyProjectMaterial = true;
            this.ApplyToAllAreas = true;
            this.Materials = new List<ModelMaterialCreateItemViewModel>();
            var materialData = new List<Material>();
            var currentModelMaterial = db.ModelMaterials.Include(m => m.Material)
                                                      .Distinct()
                                                      .Where(m => m.ModelID == this.ModelId)
                                                      .Select(m => m.Material);
            if (this.OnlyProjectMaterial)
            {
                var materialExceptCurrentInProject = db.ModelMaterials.Include(m => m.Material)
                                                          .Include(m => m.Model)
                                                          .Distinct()
                                                          .Where(m => m.Model.ProjectID == this.ProjectId)
                                                          .Select(m => m.Material)
                                                          .Except(currentModelMaterial)
                                                          .OrderBy(m => m.Designation)
                                                          .ToList();
                materialData = materialExceptCurrentInProject.ToList();
            }
            else
            {
                var materialExceptCurrent = db.Materials.Except(currentModelMaterial)
                                                .OrderBy(m => m.Designation);
                materialData = materialExceptCurrent.ToList();
            }
            foreach (var item in materialData)
            {
                this.Materials.Add(new ModelMaterialCreateItemViewModel(this.ModelId, item.MaterialID));
            }
        }

        //Methods
        public Guid Create()
        {
            foreach (var item in this.Materials)
            {
                if (item.Quantity > 0)
                {
                    var modelMaterial = new ModelMaterial();
                    modelMaterial.ModelID = item.ModelId;
                    modelMaterial.MaterialID = item.MaterialId;
                    modelMaterial.Quantity = item.Quantity;
                    db.ModelMaterials.Add(modelMaterial);
                    if (this.ApplyToAllAreas) //test to change existing Areas that reference this Model
                    {
                        var areas = db.Areas.Include(a => a.AreaMaterials)
                                            .Where(a => a.ModelID == item.ModelId)
                                            .ToList();
                        foreach (var a in areas)
                        {
                            if (a.AreaMaterials.Any(am => am.AreaID == a.AreaID & am.MaterialID == item.MaterialId)) //adjust Quantity for existing AreaMaterial
                            {
                                var existingAreaMaterial = db.AreaMaterials.FirstOrDefault(am => am.AreaID == a.AreaID & am.MaterialID == item.MaterialId);
                                existingAreaMaterial.Quantity = item.Quantity;
                                db.Entry(existingAreaMaterial).State = EntityState.Modified;
                            }
                            else //add AreaMaterial to Areas
                            {
                                var newAreaMaterial = new AreaMaterial
                                {
                                    AreaID = a.AreaID, 
                                    MaterialID = item.MaterialId, 
                                    Quantity = item.Quantity
                                };
                                db.AreaMaterials.Add(newAreaMaterial);
                            }
                        }
                    }
                }
            }
            db.SaveChanges();
            return this.ModelId;
        }
    }

    public class ModelMaterialCreateItemViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        [Key, Column(Order = 0)]
        public Guid ModelId { get; set; }
        [Key, Column(Order = 1)]
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; }
        [DataType(DataType.MultilineText), Display(ShortName = "Desc.")]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public string UnitOfMeasureName { get; set; }
        [Range(0, 99999)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F0}", HtmlEncode = false)]
        public int Quantity { get; set; }

        //Constructors
        public ModelMaterialCreateItemViewModel() { }

        public ModelMaterialCreateItemViewModel(Guid modelId, Guid materialId)
        {
            var materialData = db.Materials.Find(materialId);
            this.ModelId = modelId;
            this.MaterialId = materialId;
            this.MaterialName = materialData.Designation;
            this.Description = materialData.Description;
            this.ImagePath = materialData.ImagePath;
            this.UnitOfMeasureName = materialData.UnitOfMeasure.Designation;
            this.Quantity = 0;
        }
    }
}