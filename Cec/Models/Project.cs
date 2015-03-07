using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cec.Models
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Project ID", ShortName = "ID")]
        public Guid ProjectID { get; set; }

        [Required()]
        [DataType(DataType.Text)]
        [Display(Name = "Project")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string Designation { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        [StringLength(20, ErrorMessage = "Cannot be longer than 20 characters.")]
        [Display(Name = "Purchase Order", ShortName = "PO")]
        [DisplayFormat(NullDisplayText = "-")]
        public string PurchaseOrder { get; set; }

        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Cannot be longer than 100 characters.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string City { get; set; }

        [DataType(DataType.Text)]
        [StringLength(2, ErrorMessage = "Cannot be longer than 2 characters.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string State { get; set; }

        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Please enter a 5 digit code.")]
        [Display(Name = "Postal Code", ShortName = "Zip", Prompt = "Enter postal code")]
        [DisplayFormat(NullDisplayText = "-")]
        public Nullable<int> PostalCode { get; set; }

        public virtual ICollection<Building> Buildings { get; set; }
        public virtual ICollection<Model> Models { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}