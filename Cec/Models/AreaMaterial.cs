using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cec.Models
{
    public class AreaMaterial
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "Area")]
        public Guid AreaID { get; set; }

        [Key]
        [Column(Order = 1)]
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        [Range(0.01, 10000, ErrorMessage = "Must be between 0.01 and 10,000.00.")]
        [Required(ErrorMessage = "Quantity is required.")]
        public double Quantity { get; set; }

        public virtual Area Area { get; set; }
        public virtual Material Material { get; set; }
    }
}