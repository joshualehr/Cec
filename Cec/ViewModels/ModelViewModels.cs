using Cec.Models;
using System;
using System.Collections.Generic;
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
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Models.Where(m => m.ProjectID == projectId)
                            .OrderBy(m => m.Designation)
                            .Select(m => new SelectListItem {
                                Value = m.ModelID.ToString(),
                                Text = m.Designation
                            });
        }
    }

    public class ModelIndexViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ICollection<ModelIndexItemViewModel> Models { get; set; }

        //Constructors
        public ModelIndexViewModel() { }

        public ModelIndexViewModel(Guid projectId)
        {
            var projectData = db.Projects.Find(projectId);
            this.ProjectId = projectData.ProjectID;
            this.ProjectName = projectData.Designation;
            this.Models = new List<ModelIndexItemViewModel>();
            foreach (var item in projectData.Models.OrderBy(m => m.Designation))
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
        public string OriginalModel { get; set; }
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
            this.OriginalModel = modelData.Designation;
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

    public class ModelMaterialIndexViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        public string Project { get; set; }

        public Guid ModelId { get; set; }

        public string Model { get; set; }

        public Guid MaterialId { get; set; }

        public string Material { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public double Quantity { get; set; }

        [Display(Name = "Rough-In Quantity", ShortName = "Rough Qty")]
        public double RoughQuantity { get; set; }

        [Display(Name = "Finish Quantity", ShortName = "Finish Qty")]
        public double FinishQuantity { get; set; }

        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public ModelMaterialIndexViewModel() { }

        public ModelMaterialIndexViewModel(ModelMaterial modelMaterial)
        {
            ProjectId = modelMaterial.Model.ProjectID;
            Project = modelMaterial.Model.Project.Designation;
            ModelId = modelMaterial.ModelID;
            Model = modelMaterial.Model.Designation;
            MaterialId = modelMaterial.MaterialID;
            Material = modelMaterial.Material.Designation;
            ImagePath = modelMaterial.Material.ImagePath;
            Quantity = modelMaterial.Quantity;
            RoughQuantity = modelMaterial.RoughQuantity;
            FinishQuantity = modelMaterial.FinishQuantity;
            UnitOfMeasure = modelMaterial.Material.UnitOfMeasure.Designation;
        }

        //Methods
        public List<ModelMaterialIndexViewModel> ListByModel(Guid modelId)
        {
            var modelMaterials = db.ModelMaterials.Where(mm => mm.ModelID == modelId)
                                                  .OrderBy(mm => mm.Material.Designation)
                                                  .ToList();
            if (modelMaterials.Count() > 0)
            {
                var modelMaterialIndexViewModels = new List<ModelMaterialIndexViewModel>();
                foreach (var modelMaterial in modelMaterials)
                {
                    var modelMaterialIndexViewModel = new ModelMaterialIndexViewModel(modelMaterial);
                    modelMaterialIndexViewModels.Add(modelMaterialIndexViewModel);
                }
                return modelMaterialIndexViewModels;
            }
            else
            {
                return null;
            }
        }
    }

    public class ModelMaterialCreateViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }
        public string Project { get; set; }
        public Guid ModelId { get; set; }
        public string Model { get; set; }
        public bool ApplyToAllAreas { get; set; }
        public List<ModelMaterialCreateItemViewModel> Materials { get; set; }

        //Constructors
        public ModelMaterialCreateViewModel()
        {
            Materials = new List<ModelMaterialCreateItemViewModel>();
        }

        public ModelMaterialCreateViewModel(Guid modelId)
        {
            var modelData = db.Models.Find(modelId);
            ProjectId = modelData.ProjectID;
            Project = modelData.Project.Designation;
            ModelId = modelData.ModelID;
            Model = modelData.Designation;
            ApplyToAllAreas = true;
            Materials = new List<ModelMaterialCreateItemViewModel>();
            var currentModelMaterial = db.ModelMaterials.Include(m => m.Material)
                                                        .Where(m => m.ModelID == ModelId)
                                                        .Select(m => m.Material)
                                                        .Distinct();
            var materialExceptCurrent = db.Materials.Except(currentModelMaterial)
                                                    .OrderBy(m => m.Designation)
                                                    .ToList();
            foreach (var item in materialExceptCurrent)
            {
                Materials.Add(new ModelMaterialCreateItemViewModel(ModelId, item.MaterialID));
            }
        }

        //Methods
        public Guid Create()
        {
            foreach (var m in Materials)
            {
                if (m.RoughQuantity > 0 | m.FinishQuantity > 0)
                {
                    db.ModelMaterials.Add(new ModelMaterial()
                    {
                        ModelID = m.ModelId,
                        MaterialID = m.MaterialId,
                        Quantity = m.RoughQuantity + m.FinishQuantity,
                        RoughQuantity = m.RoughQuantity,
                        FinishQuantity = m.FinishQuantity
                    });
                    if (ApplyToAllAreas) //test to change existing Areas that reference this Model
                    {
                        var areas = db.Areas.Where(a => a.ModelID == m.ModelId).ToList();
                        foreach (var a in areas)
                        {
                            db.AreaMaterials.Add(new AreaMaterial()
                            {
                                AreaID = a.AreaID,
                                MaterialID = m.MaterialId,
                                Quantity = m.RoughQuantity + m.FinishQuantity,
                                RoughQuantity = m.RoughQuantity,
                                FinishQuantity = m.FinishQuantity
                            });
                        }
                    }
                }
            }
            db.SaveChanges();
            return ModelId;
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

        [Display(Name = "Material")]
        public string MaterialName { get; set; }

        [DataType(DataType.MultilineText), Display(ShortName = "Desc.")]
        public string Description { get; set; }

        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public string UnitOfMeasureName { get; set; }

        [Range(0, 99999)]
        [Display(Name = "Rough-In Quantity", ShortName = "Rough Qty")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F0}", HtmlEncode = false)]
        public double RoughQuantity { get; set; }

        [Range(0, 99999)]
        [Display(Name = "Finish Quantity", ShortName = "Finish Qty")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F0}", HtmlEncode = false)]
        public double FinishQuantity { get; set; }

        //Constructors
        public ModelMaterialCreateItemViewModel() { }

        public ModelMaterialCreateItemViewModel(Guid modelId, Guid materialId)
        {
            var materialData = db.Materials.Find(materialId);
            ModelId = modelId;
            MaterialId = materialId;
            MaterialName = materialData.Designation;
            Description = materialData.Description;
            UnitOfMeasureName = materialData.UnitOfMeasure.Designation;
            RoughQuantity = 0;
            FinishQuantity = 0;
        }
    }

    public class ModelMaterialEditViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        //Public Properties
        public Guid ProjectId { get; set; }

        public string Project { get; set; }

        public Guid ModelId { get; set; }

        public string Model { get; set; }

        public Guid MaterialId { get; set; }

        public string Material { get; set; }

        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        public string UnitOfMeasure { get; set; }

        public string Quantity { get; set; }

        [Range(0, 99999)]
        [Display(Name = "Rough-In Quantity", ShortName = "Rough Qty")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F0}", HtmlEncode = false)]
        public double RoughQuantity { get; set; }

        [Range(0, 99999)]
        [Display(Name = "Finish Quantity", ShortName = "Finish Qty")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F0}", HtmlEncode = false)]
        public double FinishQuantity { get; set; }

        public bool ApplyToExisting { get; set; }

        //Constructors
        public ModelMaterialEditViewModel() { }

        public ModelMaterialEditViewModel(Guid modelId, Guid materialId)
        {
            var modelMaterial = db.ModelMaterials.Find(modelId, materialId);
            ProjectId = modelMaterial.Model.ProjectID;
            Project = modelMaterial.Model.Project.Designation;
            ModelId = modelMaterial.ModelID;
            Model = modelMaterial.Model.Designation;
            MaterialId = modelMaterial.MaterialID;
            Material = modelMaterial.Material.Designation;
            UnitOfMeasure = modelMaterial.Material.UnitOfMeasure.Designation;
            Quantity = modelMaterial.Quantity.ToString();
            RoughQuantity = modelMaterial.RoughQuantity;
            FinishQuantity = modelMaterial.FinishQuantity;
            ApplyToExisting = true;
        }

        //Methods
        public Guid Edit(bool applyToExisting)
        {
            var modelMaterial = new ModelMaterial()
            {
                ModelID = ModelId,
                MaterialID = MaterialId,
                Quantity = RoughQuantity + FinishQuantity,
                RoughQuantity = RoughQuantity,
                FinishQuantity = FinishQuantity
            };
            db.Entry(modelMaterial).State = EntityState.Modified;
            if (applyToExisting)
            {
                var areaMaterials = db.AreaMaterials.Where(a => a.Area.ModelID == ModelId && a.MaterialID == MaterialId);
                foreach (var am in areaMaterials)
                {
                    am.Quantity = RoughQuantity + FinishQuantity;
                    am.RoughQuantity = RoughQuantity;
                    am.FinishQuantity = FinishQuantity;
                    db.Entry(am).State = EntityState.Modified;
                }
            }
            db.SaveChanges();
            return ModelId;
        }
    }
}