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
    public class AreaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Area/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Index(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var areas = new AreaIndexViewModel().ListByBuilding(id);
            if (areas != null)
            {
                return View(areas);
            }
            else
            {
                return RedirectToAction("Create", new { id = id });
            }
        }

        // POST: /Area/5
        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Index(AreaIndexViewModel[] areas)
        {
            var areaList = new List<AreaIndexViewModel>();
            var areasStr = new System.Text.StringBuilder();
            foreach (var area in areas)
            {
                if (area.Selected == true)
                {
                    areaList.Add(area);
                    areasStr.Append(area.Area);
                    areasStr.Append("-");
                }
            }
            if (areaList.Count() < 1)
            {
                ModelState.AddModelError("noneSelected", "No areas selected. Please select at least one area.");
                return View(areas.ToList());
            }
            TempData["aivm"] = areaList;
            TempData["areasString"] = areasStr.Remove(areasStr.Length - 1, 1).ToString();
            return RedirectToAction("AreasMaterial");
        }

        // GET: /Area/Details/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var area = new AreaDetailsViewModel(id ?? Guid.Empty);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // GET: /Area/Create/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new AreaCreateViewModel(id ?? Guid.Empty));
        }

        // POST: /Area/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,AreaDesignation,Description,Address,City,State,PostalCode,StatusId,ModelId")] AreaCreateViewModel area)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = area.Create() });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(area);
        }

        // GET: /Area/Edit/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var area = new AreaEditViewModel(id ?? Guid.Empty);
            if (area == null)
            {
                return HttpNotFound();
            }
            Session["OriginalModelId"] = area.ModelId;
            return View(area);
        }

        // POST: /Area/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,AreaId,AreaDesignation,Description,Address,City,State,PostalCode,ModelId,StatusId")] AreaEditViewModel area)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var originalModelId = (Guid?)Session["OriginalModelId"];
                    return RedirectToAction("Details", new { id = area.Edit(originalModelId ?? null) });
                }

            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(area);
        }

        // GET: /Area/Copy/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Copy(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var area = new AreaCopyViewModel(id ?? Guid.Empty);
            if (area == null)
            {
                return HttpNotFound();
            }
            area.AreaDesignation += " Copy";
            return View(area);
        }

        // POST: /Area/Copy/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Copy([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,AreaId,AreaDesignation,Description,Address,City,State,PostalCode,ModelId")] AreaCopyViewModel area)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = area.Copy(area.BuildingId) });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(area);
        }

        // GET: /Area/Delete/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var area = new AreaDeleteViewModel(id ?? Guid.Empty);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: /Area/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DeleteConfirmed([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,AreaId,AreaDesignation")] AreaDeleteViewModel area)
        {
            try
            {
                return RedirectToAction("Index", new { id = area.Delete() });
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Delete failed. Try again, and if the problem persists see your system administrator.");
                return View(area);
            }
        }

        //GET: /Area/AreasMaterial
        public ActionResult AreasMaterial()
        {
            var areaIndexViewModels = TempData["areaList"] as List<AreaIndexViewModel>;
            var areasStr = TempData["areasString"];
            if (areaIndexViewModels == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["areasStr"] = areasStr;
            return View(new AreasMaterialViewModel().ListByAreas(areaIndexViewModels));
        }

        //POST: /Area/AreasMaterial
        [HttpPost, ActionName("AreasMaterial")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DownloadData(AreasMaterialViewModel[] areasMaterialViewModels)
        {
            try
            {
                var lamcvm = new List<AreasMaterialCsvViewModel>();
                var areasStr = TempData["areasStr"].ToString();
                foreach (var item in areasMaterialViewModels)
                {
                    if (item.Selected)
                    {
                        var amcvm = new AreasMaterialCsvViewModel(item);
                        amcvm.Areas = areasStr;
                        lamcvm.Add(amcvm);
                    }
                }
                if (lamcvm.Count() > 0)
                {
                    var fileName = "Areas-" + areasStr + "-Material-" + DateTime.Now.Year.ToString() + DateTime.Now.DayOfYear.ToString() + ".csv";
                    return new CsvActionResult<AreasMaterialCsvViewModel>(lamcvm, fileName);
                }
                else
                {
                    ModelState.AddModelError("", "No items selected. Please select items to download.");
                    return View(areasMaterialViewModels.ToList());
                }
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to download at this time. Try again, and if the problem persists see your system administrator.");
                return View(areasMaterialViewModels.ToList());
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
