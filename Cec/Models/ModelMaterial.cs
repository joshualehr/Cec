using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class ModelMaterial
    {
        [Key]
        [Column(Order = 0)]
        public Guid ModelID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid MaterialID { get; set; }

        public double Quantity { get; set; }

        public double RoughQuantity { get; set; }

        public double FinishQuantity { get; set; }

        public virtual Material Material { get; set; }

        public virtual Model Model { get; set; }
    }
}