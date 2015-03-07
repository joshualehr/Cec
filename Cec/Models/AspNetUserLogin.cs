using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cec.Models
{
    public class AspNetUserLogin
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [Display(Name = "Login Provider")]
        public string LoginProvider { get; set; }

        [Display(Name = "Provider Key")]
        public string ProviderKey { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}