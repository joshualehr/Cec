using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    public class Contact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ContactID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Company { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Trade { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Chat { get; set; }

        public string Website { get; set; }

        public virtual ICollection<ProjectContact> Projects { get; set; }
    }
}