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
            var aivm = new List<AreaIndexViewModel>();
            var areasStr = new System.Text.StringBuilder();
            foreach (var area in areas)
            {
                if (area.Selected == true)
                {
                    aivm.Add(area);
                    areasStr.Append(area.Area);
                    areasStr.Append("-");
                }
            }
            if (aivm.Count() < 1)
            {
                ModelState.AddModelError("noneSelected", "No areas selected. Please select at least one area.");
                return View(areas.ToList());
            }
            TempData["aivm"] = aivm;
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
            var areaDetailsViewModel = new AreaDetailsViewModel(id ?? Guid.Empty);
            if (areaDetailsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(areaDetailsViewModel);
        }

        // GET: /Area/Create/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var areaCreateViewModel = new AreaCreateViewModel(id ?? Guid.Empty);
            return View(areaCreateViewModel);
        }

        // POST: /Area/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,AreaDesignation,Description,Address,City,State,PostalCode,StatusId,ModelId")] AreaCreateViewModel areaCreateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = areaCreateViewModel.Create() });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(areaCreateViewModel);
        }

        // GET: /Area/Edit/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var areaEditViewModel = new AreaEditViewModel(id ?? Guid.Empty);
            if (areaEditViewModel == null)
            {
                return HttpNotFound();
            }
            Session["OriginalModelId"] = areaEditViewModel.ModelId;
            return View(areaEditViewModel);
        }

        // POST: /Area/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,AreaId,AreaDesignation,Description,Address,City,State,PostalCode,ModelId,StatusId")] AreaEditViewModel areaEditViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Guid originalModelId = (Guid)Session["OriginalModelId"];
                    return RedirectToAction("Details", new { id = areaEditViewModel.Edit(originalModelId) });
                }

            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(areaEditViewModel);
        }

        // GET: /Area/Copy/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Copy(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var areaId = id ?? Guid.Empty;
            var areaCopyViewModel = new AreaCopyViewModel(areaId);
            if (areaCopyViewModel == null)
            {
                return HttpNotFound();
            }
            return View(areaCopyViewModel);
        }

        // POST: /Area/Copy/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Copy([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,AreaId,AreaDesignation,Description,Address,City,State,PostalCode,ModelId,StatusId")] AreaCopyViewModel areaCopyViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = areaCopyViewModel.Copy() });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(areaCopyViewModel);
        }

        // GET: /Area/Delete/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var areaDeleteViewModel = new AreaDeleteViewModel(id ?? Guid.Empty);
            if (areaDeleteViewModel == null)
            {
                return HttpNotFound();
            }
            return View(areaDeleteViewModel);
        }

        // POST: /Area/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DeleteConfirmed([Bind(Include = "ProjectId,ProjectDesignation,BuildingId,BuildingDesignation,AreaId,AreaDesignation")] AreaDeleteViewModel areaDeleteViewModel)
        {
            try
            {
                return RedirectToAction("Index", new { id = areaDeleteViewModel.Delete() });
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Delete failed. Try again, and if the problem persists see your system administrator.");
                return View(areaDeleteViewModel);
            }
        }

        //GET: /Area/AreasMaterial
        public ActionResult AreasMaterial()
        {
            var areaIndexViewModels = TempData["aivm"] as List<AreaIndexViewModel>;
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
