using Cec.Helpers;
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
    [Authorize(Roles = "canAdminister")]
    public class AreaController : Controller
    {
        // GET: /Area/5
        public ActionResult Index(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var areaIndexViewModel = new AreaIndexViewModel(id);
            if (areaIndexViewModel.Areas != null)
            {
                return View(areaIndexViewModel);
            }
            else
            {
                return RedirectToAction("Create", new { id = id });
            }
        }

        // POST: /Area/5
        [HttpPost, ActionName("Index"), ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,Building,Areas")] AreaIndexViewModel areaIndexViewModel)
        {
            if (areaIndexViewModel.Areas.Any(a => a.Selected))
            {
                TempData["areaIndexViewModel"] = areaIndexViewModel;
                return RedirectToAction("AreasMaterial");
            }
            else
            {
                ModelState.AddModelError("noneSelected", "No areas selected. Please select at least one area.");
                return View(areaIndexViewModel);
            }
        }

        // GET: /Area/Details/5
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
        [HttpPost, ValidateAntiForgeryToken]
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
        [HttpPost, ValidateAntiForgeryToken]
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
            return View(area);
        }

        // POST: /Area/Copy/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
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
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
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
            var areaIndexViewModel = TempData["areaIndexViewModel"] as AreaIndexViewModel;
            if (areaIndexViewModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                return View(new AreasMaterialViewModel(areaIndexViewModel));
            }
        }

        //POST: /Area/AreasMaterial
        [HttpPost, ActionName("AreasMaterial"), ValidateAntiForgeryToken]
        public ActionResult DownloadData([Bind(Include = "ProjectId,Project,BuildingId,Building,Areas,Materials")] AreasMaterialViewModel areasMaterialViewModel)
        {
            try
            {
                if (areasMaterialViewModel.Materials.Any(m => m.Selected))
                {
                    var fileName = "Areas-Material-" + DateTime.Now.Year.ToString() + DateTime.Now.DayOfYear.ToString() + ".csv";
                    return new CsvActionResult<AreasMaterialCsvItemViewModel>(new AreasMaterialCsvViewModel(areasMaterialViewModel).Materials, fileName);
                }
                else
                {
                    ModelState.AddModelError("", "No items selected. Please select items to download.");
                    return View(areasMaterialViewModel);
                }
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to download at this time. Try again, and if the problem persists see your system administrator.");
                return View(areasMaterialViewModel);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var  db = new ApplicationDbContext();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
