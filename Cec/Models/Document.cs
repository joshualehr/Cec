using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cec.Models
{
    public class Document
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Document ID", ShortName = "ID")]
        public Guid DocumentID { get; set; }

        [Required()]
        [Display(Name = "Document", ShortName = "Doc.")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string Designation { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        [Url()]
        [Required()]
        [DataType(DataType.Url, ErrorMessage = "Please enter a vailid link.")]
        [Display(Name = "File Link", ShortName = "File")]
        public string FileLink { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Building> Buildings { get; set; }
        public virtual ICollection<Model> Models { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}