using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProjectID { get; set; }

        [Required()]
        [StringLength(50)]
        public string Designation { get; set; }

        public string Description { get; set; }

        [Index(IsClustered = false, IsUnique = false)]
        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(20)]
        public string PurchaseOrder { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        public int? PostalCode { get; set; }

        public virtual ICollection<Building> Buildings { get; set; }

        public virtual ICollection<Model> Models { get; set; }

        public virtual ICollection<ProjectContact> Contacts { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}