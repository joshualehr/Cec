using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class Model
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Model ID", ShortName = "ID")]
        public Guid ModelID { get; set; }

        [Required()]
        [DataType(DataType.Text)]
        [Display(Name = "Model")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        public string Designation { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        [Index(IsClustered = false, IsUnique = false)]
        [Required()]
        [Display(Name = "Project ID")]
        public Guid ProjectID { get; set; }

        public virtual ICollection<Area> Areas { get; set; }

        public virtual Project Project { get; set; }

        public virtual ICollection<ModelMaterial> ModelMaterials { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}