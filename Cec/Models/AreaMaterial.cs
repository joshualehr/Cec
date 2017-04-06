using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class AreaMaterial
    {
        [Key]
        [Column(Order = 0)]
        public Guid AreaID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid MaterialID { get; set; }

        public double Quantity { get; set; }

        public double RoughQuantity { get; set; }

        public double FinishQuantity { get; set; }

        public virtual Area Area { get; set; }

        public virtual Material Material { get; set; }
    }
}