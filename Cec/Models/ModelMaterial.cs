using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class ModelMaterial
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "Model")]
        public Guid ModelID { get; set; }

        [Key]
        [Column(Order = 1)]
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        [Range(0.01, 10000, ErrorMessage = "Must be between 0.01 and 10,000.00.")]
        [Required(ErrorMessage = "Quantity is required.")]
        public double Quantity { get; set; }

        public virtual Material Material { get; set; }

        public virtual Model Model { get; set; }
    }
}