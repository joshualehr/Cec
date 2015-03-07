using Cec.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace Cec.ViewModels
{
    public class BuildingSelectListViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        public class _Building
        {
            public Guid BuildingId { get; set; }
            public string Designation { get; set; }
        }

        private List<_Building> _BuildingList = new List<_Building>();

        //Public Propeties
        public List<_Building> BuildingList
        {
            get { return _BuildingList; }
            set { _BuildingList = value; }
        }

        //Constructors
        public BuildingSelectListViewModel()
        {

        }

        public BuildingSelectListViewModel(Guid projectId)
        {
            var buildings = db.Buildings.Include(b => b.Project)
                                     .Where(b => b.ProjectID == projectId)
                                     .OrderBy(b => b.Designation);
            foreach (var item in buildings)
            {
                var building = new _Building()
                {
                    BuildingId = item.BuildingID,
                    Designation = item.Designation
                };
                this.BuildingList.Add(building);
            }
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