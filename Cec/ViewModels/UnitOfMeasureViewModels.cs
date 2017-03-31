using Cec.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cec.ViewModels
{
    public class UnitOfMeasureSelectListViewModel
    {
        //Private Properties
        private ApplicationDbContext db = new ApplicationDbContext();

        public class _UnitOfMeasure
        {
            public Guid UnitOfMeasureId { get; set; }
            public string UnitOfMeasure { get; set; }
        }

        private List<_UnitOfMeasure> _UnitsOfMeasureList = new List<_UnitOfMeasure>();

        //Public Propeties
        public List<_UnitOfMeasure> UnitsOfMeasureList
        {
            get { return _UnitsOfMeasureList; }
            set { _UnitsOfMeasureList = value; }
        }

        //Constructors
        public UnitOfMeasureSelectListViewModel()
        {
            var unitsOfMeasure = db.UnitOfMeasures.OrderBy(u => u.Designation);
            foreach (var item in unitsOfMeasure)
            {
                var unitOfMeasure = new _UnitOfMeasure()
                {
                    UnitOfMeasureId = item.UnitOfMeasureID,
                    UnitOfMeasure = item.Designation
                };
                this.UnitsOfMeasureList.Add(unitOfMeasure);
            }
        }
    }
}