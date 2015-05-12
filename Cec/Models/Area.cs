using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class Area
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AreaID { get; set; }

        [Required]
        [StringLength(50)]
        public string Designation { get; set; }

        public string Description { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        public Nullable<int> PostalCode { get; set; }

        [Required()]
        [Index(IsClustered = false, IsUnique = false)]
        public Guid BuildingID { get; set; }

        [Index(IsClustered = false, IsUnique = false)]
        public Guid? ModelID { get; set; }

        [Required()]
        [Index(IsClustered = false, IsUnique = false)]
        public Guid StatusId { get; set; }

        public DateTime StatusChanged { get; set; }

        public virtual Building Building { get; set; }

        public virtual Model Model { get; set; }

        public virtual Status Status { get; set; }

        public virtual ICollection<AreaMaterial> AreaMaterials { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}