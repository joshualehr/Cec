using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cec.Models
{
    public class AspNetRole
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}