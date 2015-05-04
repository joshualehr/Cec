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
        public ActionResult Index(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var buildings = new BuildingIndexViewModel().ListByProject(id);
            if (buildings != null)
            {
                return View(buildings);
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
        public ActionResult Index(BuildingIndexViewModel[] buildings)
        {
            var buildingList = new List<BuildingIndexViewModel>();
            foreach (var building in buildings)
            {
                if (building.Selected == true)
                {
                    buildingList.Add(building);
                }
            }
            if (buildingList.Count() < 1)
            {
                ModelState.AddModelError("noneSelected", "No buildings selected. Please select at least one building.");
                return View(buildings.ToList());
            }
            TempData["buildingList"] = buildingList;
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
                    return RedirectToAction("Details", new { id = building.Copy() });
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
            var buildings = TempData["buildingList"] as List<BuildingIndexViewModel>;
            if (buildings == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           return View(new BuildingsMaterialViewModel().ListByBuildings(buildings));
        }

        // POST: /Building/BuildingsMaterial
        [HttpPost, ActionName("BuildingsMaterial")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DownloadData(BuildingsMaterialViewModel[] buildingsMaterial)
        {
            try
            {
                var model = new List<BuildingsMaterialCsvViewModel>();
                foreach (var item in buildingsMaterial)
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
                    return View(buildingsMaterial.ToList());
                }
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to download at this time. Try again, and if the problem persists see your system administrator.");
                return View(buildingsMaterial.ToList());
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
