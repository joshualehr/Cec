using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cec.Models
{
    public class Material
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Material ID", ShortName = "ID")]
        public Guid MaterialID { get; set; }

        [Required()]
        [DataType(DataType.Text)]
        [Display(Name = "Material")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string Designation { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image Path", ShortName = "Image")]
        [DisplayFormat(NullDisplayText = "-")]
        public string ImagePath { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public int? Barcode { get; set; }

        [Index(IsClustered = false, IsUnique = false)]
        [Required(ErrorMessage = "Unit of measure is required.")]
        [Display(Name = "U/M")]
        public Guid? UnitOfMeasureID { get; set; }

        public virtual ICollection<AreaMaterial> AreaMaterials { get; set; }
        public virtual ICollection<ModelMaterial> ModelMaterials { get; set; }
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
    }
}