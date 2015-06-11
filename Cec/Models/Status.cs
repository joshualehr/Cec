using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cec.Models
{
    public class Status
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StatusId { get; set; }

        [Required()]
        [StringLength(20)]
        public string Designation { get; set; }

        public Int16 ListOrder { get; set; }

        public virtual ICollection<Area> Area { get; set; }
    }
}