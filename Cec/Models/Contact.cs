using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cec.Models
{
    public class Contact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Contact")]
        public Guid ContactID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Company { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Trade { get; set; }

        [Phone()]
        [DataType(DataType.PhoneNumber, ErrorMessage="Please enter a valid phone number.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Phone { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress()]
        [DataType(DataType.EmailAddress, ErrorMessage="Please enter a valid email.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Email { get; set; }

        [Url()]
        [DataType(DataType.Url, ErrorMessage="Please enter a vailid link address.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Chat { get; set; }

        [Url()]
        [DataType(DataType.Url, ErrorMessage = "Please enter a vailid web address.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Website { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}