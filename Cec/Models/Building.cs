using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class Building
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BuildingID { get; set; }

        [Required()]
        [StringLength(50)]
        public string Designation { get; set; }

        public string Description { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        public int? PostalCode { get; set; }

        [Index(IsClustered = false, IsUnique = false)]
        [Required()]
        public Guid ProjectID { get; set; }

        public virtual ICollection<Area> Areas { get; set; }

        public virtual Project Project { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}