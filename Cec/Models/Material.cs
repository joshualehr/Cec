using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cec.Models
{
    [SerializePropertyNamesAsCamelCase]
    public class Material
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Material ID", ShortName = "ID")]
        public Guid MaterialID { get; set; }

        [Required()]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
        [Display(Name = "Material")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters or shorter than 2.", MinimumLength = 2)]
        public string Designation { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(ShortName = "Desc.")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.ImageUrl)]
        [Display(Name = "Image Path", ShortName = "Image")]
        [DisplayFormat(NullDisplayText = "-")]
        public string ImagePath { get; set; }

        [DisplayFormat(NullDisplayText = "-")]
        public int? Barcode { get; set; }

        [Index(IsClustered = false, IsUnique = false)]
        [Required(ErrorMessage = "Unit of measure is required.")]
        [Display(Name = "U/M")]
        public Guid? UnitOfMeasureID { get; set; }

        public virtual ICollection<AreaMaterial> AreaMaterials { get; set; }

        public virtual ICollection<ModelMaterial> ModelMaterials { get; set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        public static IList<Field> GetSearchableFields()
        {
            return new List<Field>()
            {
                new Field("materialId", Microsoft.Azure.Search.Models.DataType.String)  { IsKey = true },
                new Field("designation", Microsoft.Azure.Search.Models.DataType.String) { IsSearchable = true, IsSortable = true, IsFilterable = true },
                new Field("description", Microsoft.Azure.Search.Models.DataType.String) { IsSearchable = true }
            };
        }

        public Microsoft.Azure.Search.Models.Document AsSearchDocument()
        {
            return new Microsoft.Azure.Search.Models.Document()
            {
                { "materialId", MaterialID },
                { "designation", Designation },
                { "description", Description }
            };
        }
    }
}