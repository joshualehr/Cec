﻿using Cec.Helpers;
using Cec.Models;
using Cec.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Controllers
{
    [Authorize(Roles = "isEmployee")]
    public class BuildingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Building/(BuildingId)
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

        // POST: /Building/(BuildingId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Index([Bind(Include = "ProjectId,ProjectDesignation,Buildings")] BuildingIndexViewModel buildingIndexViewModel)
        {
            var buildings = new List<BuildingIndexItemViewModel>();
            foreach (var building in buildingIndexViewModel.Buildings)
            {
                if (building.Selected)
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

        // GET: /Building/Details/(BuildingId)
        [Authorize(Roles = "canViewDetails")]
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

        // GET: /Building/Create/(ProjectId)
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new BuildingCreateViewModel(id ?? Guid.Empty));
        }

        // POST: /Building/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Create([Bind(Include = "ProjectId,ProjectDesignation,BuildingDesignation,Description,Address,City,State,PostalCode")] BuildingCreateViewModel building)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = building.Create() });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(building);
        }

        // GET: /Building/Edit/(BuildingId)
        [Authorize(Roles = "canManageProjects")]
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

        // POST: /Building/Edit/(BuildingId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,Description,Address,City,State,PostalCode")] BuildingEditViewModel building)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = building.Edit() });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(building);
        }

        // GET: /Building/Copy/(BuildingId)
        [Authorize(Roles = "canManageProjects")]
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

        // POST: /Building/Copy/(BuildingId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Copy([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,Description,Address,City,State,PostalCode")] BuildingCopyViewModel building)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = building.Copy(building.BuildingId) });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(building);
        }

        // GET: /Building/Delete/5
        [Authorize(Roles = "canManageProjects")]
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

        // POST: /Building/Delete/(BuildingId)
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult DeleteConfirmed([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation")] BuildingDeleteViewModel building)
        {
            try
            {
                return RedirectToAction("Index", new { id = building.Delete() });

            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Delete failed. Try again, and if the problem persists see your system administrator.");
                return View(building);
            }
        }

        // GET: /Building/BuildingsMaterial
        [Authorize(Roles = "canManageProjects")]
        public ActionResult BuildingsMaterial()
        {
            var buildingIndexViewModel = TempData["buildingIndexViewModel"] as BuildingIndexViewModel;
            if (buildingIndexViewModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                return View(new BuildingsMaterialViewModel(buildingIndexViewModel));
            }
        }

        // POST: /Building/BuildingsMaterial
        [HttpPost]
        [ActionName("BuildingsMaterial")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult DownloadData([Bind(Include = "ProjectId,Project,Buildings,Materials")] BuildingsMaterialViewModel buildingsMaterialViewModel)
        {
            try
            {
                if (buildingsMaterialViewModel.Materials.Any(m => m.Selected))
                {
                    var fileName = buildingsMaterialViewModel.Project + "-Material Requisition-" + DateTime.Now.Year.ToString() + DateTime.Now.DayOfYear.ToString() + ".csv";
                    return new CsvActionResult<BuildingsMaterialCsvItemViewModel>(new BuildingsMaterialCsvViewModel(buildingsMaterialViewModel).Materials, fileName);
                }
                else
                {
                    ModelState.AddModelError("", "No items selected. Please select items to download.");
                    return View(buildingsMaterialViewModel);
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to download at this time. Try again, and if the problem persists see your system administrator.");
            }
            return View(buildingsMaterialViewModel);
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
