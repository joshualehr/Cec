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

        // GET: /Building/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Index(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var buildingIndexViewModel = new BuildingIndexViewModel(id ?? Guid.Empty);
            if (buildingIndexViewModel.Buildings.Count > 0)
            {
                return View(buildingIndexViewModel);
            }
            else
            {
                return RedirectToAction("Create", new { id = id });
            }
        }

        // POST: /Building/5
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Index([Bind(Include = "ProjectId,ProjectDesignation,Buildings")] BuildingIndexViewModel buildingIndexViewModel)
        {
            var buildings = new List<BuildingIndexItemViewModel>();
            foreach (var building in buildingIndexViewModel.Buildings)
            {
                if (building.Selected == true)
                {
                    buildings.Add(building);
                }
            }
            if (buildings.Count() < 1)
            {
                ModelState.AddModelError("noneSelected", "No buildings selected. Please select at least one building.");
                return View(buildingIndexViewModel);
            }
            TempData["buildingIndexViewModel"] = buildingIndexViewModel;
            return RedirectToAction("BuildingsMaterial");
        }

        // GET: /Building/Details/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var building = new BuildingDetailsViewModel(id ?? Guid.Empty);
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
            return View(new BuildingCreateViewModel(id ?? Guid.Empty));
        }

        // POST: /Building/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create([Bind(Include = "ProjectId,ProjectDesignation,BuildingDesignation,Description,Address,City,State,PostalCode")] BuildingCreateViewModel building)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = building.Create() });
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
            var building = new BuildingEditViewModel(id ?? Guid.Empty);
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
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,Description,Address,City,State,PostalCode")] BuildingEditViewModel building)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = building.Edit() });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(building);
        }

        // GET: /Building/Copy/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Copy(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var building = new BuildingCopyViewModel(id ?? Guid.Empty);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: /Building/Copy/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Copy([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,Description,Address,City,State,PostalCode")] BuildingCopyViewModel building)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = building.Copy(building.BuildingId) });
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
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var building = new BuildingDeleteViewModel(id ?? Guid.Empty);
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
        public ActionResult DeleteConfirmed([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation")] BuildingDeleteViewModel building)
        {
            try
            {
                return RedirectToAction("Index", new { id = building.Delete() });

            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Delete failed. Try again, and if the problem persists see your system administrator.");
                return View(building);
            }
        }

        // GET: /Building/BuildingsMaterial
        public ActionResult BuildingsMaterial()
        {
            var buildingIndexViewModel = TempData["buildingIndexViewModel"] as BuildingIndexViewModel;
            if (buildingIndexViewModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           return View(new BuildingsMaterialViewModel(buildingIndexViewModel));
        }

        // POST: /Building/BuildingsMaterial
        [HttpPost, ActionName("BuildingsMaterial")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DownloadData([Bind(Include = "ProjectId,ProjectDesignation,Buildings,Materials")] BuildingsMaterialViewModel buildingsMaterialViewModel)
        {
            try
            {
                var model = new BuildingsMaterialCsvViewModel().List(buildingsMaterialViewModel);
                if (model.Count() > 0)
                {
                    var fileName = model.First().Project + "-Material Requisition-" + DateTime.Now.Year.ToString() + DateTime.Now.DayOfYear.ToString() + ".csv";
                    return new CsvActionResult<BuildingsMaterialCsvViewModel>(model, fileName);
                }
                else
                {
                    ModelState.AddModelError("", "No items selected. Please select items to download.");
                    return View(buildingsMaterialViewModel);
                }
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to download at this time. Try again, and if the problem persists see your system administrator.");
                return View(buildingsMaterialViewModel);
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
