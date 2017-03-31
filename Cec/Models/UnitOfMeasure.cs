using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class UnitOfMeasure
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Unit of Measure ID", ShortName = "ID")]
        public Guid UnitOfMeasureID { get; set; }

        [Required()]
        [DataType(DataType.Text)]
        [Display(Name = "Unit of Measure", ShortName = "U/M")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string Designation { get; set; }

        public virtual ICollection<AreaMaterial> AreaMaterials { get; set; }

        public virtual ICollection<ModelMaterial> ModelMaterials { get; set; }
    }
}