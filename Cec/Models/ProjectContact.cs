using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class ProjectContact
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "Project")]
        public Guid ProjectID { get; set; }

        [Key]
        [Column(Order = 1)]
        [Display(Name = "Contact")]
        public Guid ContactID { get; set; }

        public virtual Project Project { get; set; }

        public virtual Contact Contact { get; set; }
    }
}