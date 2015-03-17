using Cec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Cec.ViewModels
{
    public class BuildingSelectList : SelectList
    {
        //Constructors
        public BuildingSelectList(Guid projectId)
            : base(items(projectId), "Value", "Text") { }

        public BuildingSelectList(Guid projectId, object selectedValue)
            : base(items(projectId), "Value", "Text", selectedValue) { }

        //Static Metods
        public static System.Collections.IEnumerable items(Guid projectId)
        {
            var db = new ApplicationDbContext();
            var selectListItems = new List<SelectListItem>();
            var buildings = db.Buildings.Where(b => b.ProjectID == projectId)
                                        .OrderBy(b => b.Designation);
            foreach (var item in buildings)
            {
                var building = new SelectListItem()
                {
                    Value = item.BuildingID.ToString(),
                    Text = item.Designation
                };
                selectListItems.Add(building);
            }
            return selectListItems;
        }
    }

    public class BuildingIndexViewModel
    {
        //Public Properties
        public Guid ProjectID { get; set; }
        public string Project { get; set; }
        public Guid BuildingID { get; set; }
        public string Building { get; set; }
        public bool Selected { get; set; }

        //Constructors
        public BuildingIndexViewModel()
        { 
            
        }

        public BuildingIndexViewModel(Building building)
        {
            this.ProjectID = building.ProjectID;
            this.Project = building.Project.Designation;
            this.BuildingID = building.BuildingID;
            this.Building = building.Designation;
        }
    }

    public class BuildingsMaterialViewModel
    {
        //Public Properties
        public bool Selected { get; set; }
        public Guid ProjectID { get; set; }
        public string Project { get; set; }
        public Guid BuildingID { get; set; }
        public string Building { get; set; }
        public string ImagePath { get; set; }
        public Guid MaterialId { get; set; }
        public string Material { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }
        [Display(Name = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public BuildingsMaterialViewModel()
        {

        }

        public BuildingsMaterialViewModel(BuildingIndexViewModel buildingIndexViewModel)
        {
            this.ProjectID = buildingIndexViewModel.ProjectID;
            this.Project = buildingIndexViewModel.Project;
            this.BuildingID = buildingIndexViewModel.BuildingID;
            this.Building = buildingIndexViewModel.Building;
        }
    }

    public class BuildingsMaterialCsvViewModel
    {
        //Public Properties
        public string Project { get; set; }
        public string Material { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}", HtmlEncode = false)]
        public double Total { get; set; }
        [Display(Name = "U/M")]
        public string UnitOfMeasure { get; set; }

        //Constructors
        public BuildingsMaterialCsvViewModel()
        {

        }

        public BuildingsMaterialCsvViewModel(BuildingsMaterialViewModel buildingsMaterialViewModel)
        {
            this.Project = buildingsMaterialViewModel.Project;
            this.Material = buildingsMaterialViewModel.Material;
            this.Total = buildingsMaterialViewModel.Total;
            this.UnitOfMeasure = buildingsMaterialViewModel.UnitOfMeasure;
        }
    }
}