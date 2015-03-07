using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cec.Models;
using Cec.ViewModels;
using System.Text;
using Cec.Helpers;

namespace Cec.Controllers
{
    public class BuildingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Building/ProjectID=5
        public ActionResult Index(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var buildings = db.Buildings.Include(b => b.Project)
                                        .Where(b => b.ProjectID == id)
                                        .OrderBy(b => b.Designation);
            if (buildings.Count() > 0)
            {
                var buildingIndexViewModels = new List<BuildingIndexViewModel>();
                foreach (var item in buildings)
                {
                    var buildingIndexViewModel = new BuildingIndexViewModel(item);
                    buildingIndexViewModels.Add(buildingIndexViewModel);
                }
                return View(buildingIndexViewModels);
            }
            else
            {
                return RedirectToAction("Create", new { id = id });
            }
        }

        // POST: /Building/5
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(BuildingIndexViewModel[] buildingIndexViewModels)
        {
            var bivm = new List<BuildingIndexViewModel>();
            foreach (var item in buildingIndexViewModels)
            {
                if (item.Selected == true)
                {
                    bivm.Add(item);
                }
            }
            if (bivm.Count() < 1)
            {
                ModelState.AddModelError("noneSelected", "No buildings selected. Please select at least one building.");
                return View(buildingIndexViewModels.ToList());
            }
            TempData["bivm"] = bivm;
            return RedirectToAction("BuildingsMaterial");
        }

        // GET: /Building/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // GET: /Building/Create/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = new Building();
            building.Project = db.Projects.Find(id);
            building.ProjectID = building.Project.ProjectID;
            return View(building);
        }

        // POST: /Building/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create([Bind(Include = "BuildingID,Designation,Description,Address,City,State,PostalCode,ProjectID")] Building building)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Buildings.Add(building);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = building.BuildingID });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(building);
        }

        // GET: /Building/Edit/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: /Building/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit([Bind(Include="BuildingID,Designation,Description,Address,City,State,PostalCode,ProjectID")] Building building)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(building).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = building.BuildingID });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(building);
        }

        // GET: /Building/Delete/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Delete(Guid? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: /Building/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                Building building = db.Buildings.Find(id);
                db.Buildings.Remove(building);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = building.ProjectID });

            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }

        // GET: /Building/BuildingsMaterial
        public ActionResult BuildingsMaterial()
        {
            var buildingIndexViewModels = TempData["bivm"] as List<BuildingIndexViewModel>;
            if (buildingIndexViewModels == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var buildingsMaterialViewModels = new List<BuildingsMaterialViewModel>();
            var areaMaterials = new List<AreaMaterial>();
            foreach (var buildingItem in buildingIndexViewModels)
            {
                areaMaterials.AddRange(db.AreaMaterials.Include(a => a.Material)
                                                       .Include(a => a.Material.UnitOfMeasure)
                                                       .Where(a => a.Area.BuildingID == buildingItem.BuildingID)
                                                       .OrderBy(a => a.Material.Designation));
            }
            var materials = areaMaterials.GroupBy(m => m.MaterialID);
            foreach (var item in materials)
            {
                var buildingsMaterialViewModel = new BuildingsMaterialViewModel();
                buildingsMaterialViewModel.ProjectID = item.First().Area.Building.ProjectID;
                buildingsMaterialViewModel.Project = item.First().Area.Building.Project.Designation;
                buildingsMaterialViewModel.BuildingID = item.First().Area.BuildingID;
                buildingsMaterialViewModel.Building = item.First().Area.Building.Designation;
                buildingsMaterialViewModel.MaterialId = item.First().MaterialID;
                buildingsMaterialViewModel.Material = item.First().Material.Designation;
                buildingsMaterialViewModel.ImagePath = item.First().Material.ImagePath;
                buildingsMaterialViewModel.UnitOfMeasure = item.First().Material.UnitOfMeasure.Designation;
                buildingsMaterialViewModel.Total = item.Sum(i => i.Quantity);
                buildingsMaterialViewModels.Add(buildingsMaterialViewModel);
            }
           return View(buildingsMaterialViewModels);
        }

        // POST: /Building/BuildingsMaterial
        [HttpPost, ActionName("BuildingsMaterial")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DownloadData(BuildingsMaterialViewModel[] buildingsMaterialViewModels)
        {
            try
            {
                var model = new List<BuildingsMaterialCsvViewModel>();
                foreach (var item in buildingsMaterialViewModels)
                {
                    if (item.Selected)
                    {
                        var bmcvm = new BuildingsMaterialCsvViewModel(item);
                        model.Add(bmcvm);
                    }
                }
                if (model.Count() > 0)
                {
                    var fileName = model.First().Project + "-Material Requisition-" + DateTime.Now.Year.ToString() + DateTime.Now.DayOfYear.ToString() + ".csv";
                    return new CsvActionResult<BuildingsMaterialCsvViewModel>(model, fileName);
                }
                else
                {
                    ModelState.AddModelError("", "No items selected. Please select items to download.");
                    return View(buildingsMaterialViewModels.ToList());
                }
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to download at this time. Try again, and if the problem persists see your system administrator.");
                return View(buildingsMaterialViewModels.ToList());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
