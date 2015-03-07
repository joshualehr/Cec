using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cec.Models
{
    public class AspNetUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string Discriminator { get; set; }

        public Nullable<System.Guid> ContactID { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
    }
}