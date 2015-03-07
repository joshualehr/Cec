using Cec.Helpers;
using Cec.Models;
using Cec.ViewModels;
using PagedList;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CompleteElectric.Controllers
{
    public class MaterialController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ImageHandler photoHandler = new ImageHandler();
        private string relativeDirectoryPath = "/Content/Images/";

        // GET: /Material/
        // paging: https://github.com/TroyGoode/PagedList
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SortOrder = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var materialIndexViewModel = new MaterialIndexViewModel();
            var materials = materialIndexViewModel.GetList(sortOrder, searchString);
            if (materials.Count() > 0)
            {

                return View(materials.ToPagedList(page ?? 1, 5));
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        // GET: /Material/Details/5
        public ActionResult Details(Guid? id, string sortOrder, string currentFilter, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var materialId = id ?? Guid.Empty;
            var materialDetailsViewModel = new MaterialDetailsViewModel(materialId);
            if (materialDetailsViewModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            return View(materialDetailsViewModel);
        }

        // GET: /Material/Create
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create(string sortOrder, string currentFilter, int? page)
        {
            var materialCreateViewModel = new MaterialCreateViewModel();
            var umslvm = new UnitOfMeasureSelectListViewModel();
            ViewBag.UnitOfMeasureID = new SelectList(umslvm.UnitsOfMeasureList, "UnitOfMeasureId", "UnitOfMeasure");
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            return View(materialCreateViewModel);
        }

        // POST: /Material/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create([Bind(Include = "Material,Description,ImagePath,UnitOfMeasureId")] MaterialCreateViewModel materialCreateViewModel, HttpPostedFileBase photo, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            try
            {
                if (ModelState.IsValid)
                {
                    if (photo != null)
                    {
                        var relativeImagePath = photoHandler.GetPossibleImagePath(photo);
                        if (photoHandler.AnyMaterialUsesImage(relativeImagePath))
                        {
                            materialCreateViewModel.ImagePath = relativeImagePath;
                        }
                        else
                        {
                            materialCreateViewModel.ImagePath = photoHandler.SaveNewImage(photo, Server.MapPath(relativeDirectoryPath));
                        }
                    }
                    return RedirectToAction("Details", new { id = materialCreateViewModel.Create(), sortOrder = ViewBag.SortOrder, currentFilter = materialCreateViewModel.Material });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            var umslvm = new UnitOfMeasureSelectListViewModel();
            ViewBag.UnitOfMeasureID = new SelectList(umslvm.UnitsOfMeasureList, "UnitOfMeasureId", "UnitOfMeasure", materialCreateViewModel.UnitOfMeasureId);
            return View(materialCreateViewModel);
        }

        // GET: /Material/Edit/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit(Guid? id, string sortOrder, string currentFilter, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var materialId = id ?? Guid.Empty;
            var materialEditViewModel = new MaterialEditViewModel(materialId);
            if (materialEditViewModel == null)
            {
                return HttpNotFound();
            }
            var umslvm = new UnitOfMeasureSelectListViewModel();
            ViewBag.UnitOfMeasureID = new SelectList(umslvm.UnitsOfMeasureList, "UnitOfMeasureId", "UnitOfMeasure", materialEditViewModel.UnitOfMeasureId);
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            return View(materialEditViewModel);
        }

        // POST: /Material/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit([Bind(Include = "MaterialID,Material,Description,ImagePath,UnitOfMeasureId")] MaterialEditViewModel materialEditViewModel, HttpPostedFileBase photo, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            try
            {
                if (ModelState.IsValid)
                {
                    if (photo != null)
                    {
                        if (materialEditViewModel.ImagePath != null && !photoHandler.OtherMaterialUsesImage(materialEditViewModel.ImagePath)) //Replacing image
                        {
                            photoHandler.DeleteImage(Server.MapPath(materialEditViewModel.ImagePath));
                        }
                        var relativeImagePath = photoHandler.GetPossibleImagePath(photo);
                        if (photoHandler.AnyMaterialUsesImage(relativeImagePath)) //Image already exist
                        {
                            materialEditViewModel.ImagePath = relativeImagePath;
                        }
                        else //Image does not exist
                        {
                            materialEditViewModel.ImagePath = photoHandler.SaveNewImage(photo, Server.MapPath(relativeDirectoryPath));
                        }
                    }
                    return RedirectToAction("Details", new { id = materialEditViewModel.Edit(), page = ViewBag.Page, sortOrder = ViewBag.SortOrder, currentFilter = ViewBag.CurrentFilter });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            var umslvm = new UnitOfMeasureSelectListViewModel();
            ViewBag.UnitOfMeasureID = new SelectList(umslvm.UnitsOfMeasureList, "UnitOfMeasureId", "UnitOfMeasure", materialEditViewModel.UnitOfMeasureId);
            return View(materialEditViewModel);
        }

        // GET: /Material/Copy/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Copy(Guid? id, string sortOrder, string currentFilter, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var materialId = id ?? Guid.Empty;
            var materialCopyViewModel = new MaterialCopyViewModel(materialId);
            if (materialCopyViewModel == null)
            {
                return HttpNotFound();
            }
            var umslvm = new UnitOfMeasureSelectListViewModel();
            ViewBag.UnitOfMeasureID = new SelectList(umslvm.UnitsOfMeasureList, "UnitOfMeasureId", "UnitOfMeasure", materialCopyViewModel.UnitOfMeasureId);
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            return View(materialCopyViewModel);
        }

        // POST: /Material/Copy/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Copy([Bind(Include = "MaterialID,Material,Description,ImagePath,UnitOfMeasureId")] MaterialCopyViewModel materialCopyViewModel, HttpPostedFileBase photo, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            try
            {
                if (ModelState.IsValid)
                {
                    if (photo != null)
                    {
                        if (materialCopyViewModel.ImagePath != null && !photoHandler.OtherMaterialUsesImage(materialCopyViewModel.ImagePath)) //Replacing image
                        {
                            photoHandler.DeleteImage(Server.MapPath(materialCopyViewModel.ImagePath));
                        }
                        var relativeImagePath = photoHandler.GetPossibleImagePath(photo);
                        if (photoHandler.AnyMaterialUsesImage(relativeImagePath)) //Image already exist
                        {
                            materialCopyViewModel.ImagePath = relativeImagePath;
                        }
                        else //Image does not exist
                        {
                            materialCopyViewModel.ImagePath = photoHandler.SaveNewImage(photo, Server.MapPath(relativeDirectoryPath));
                        }
                    }
                    return RedirectToAction("Details", new { id = materialCopyViewModel.Copy(), sortOrder = ViewBag.SortOrder, currentFilter = ViewBag.CurrentFilter });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            var umslvm = new UnitOfMeasureSelectListViewModel();
            ViewBag.UnitOfMeasureID = new SelectList(umslvm.UnitsOfMeasureList, "UnitOfMeasureId", "UnitOfMeasure", materialCopyViewModel.UnitOfMeasureId);
            return View(materialCopyViewModel);
        }

        // GET: /Material/Delete/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Delete(Guid? id, string sortOrder, string currentFilter, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var materialId = id ?? Guid.Empty;
            var materialDeleteViewModel = new MaterialDeleteViewModel(materialId);
            if (materialDeleteViewModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            return View(materialDeleteViewModel);
        }

        // POST: /Material/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DeleteConfirmed([Bind(Include = "MaterialId,ImagePath")] MaterialDeleteViewModel materialDeleteViewModel, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            try
            {
                if (!photoHandler.OtherMaterialUsesImage(materialDeleteViewModel.ImagePath)) //Image not used elsewhere
                {
                    photoHandler.DeleteImage(Server.MapPath(materialDeleteViewModel.ImagePath));
                }
                ViewBag.Page = page;
                ViewBag.SortOrder = sortOrder;
                ViewBag.CurrentFilter = currentFilter;
                materialDeleteViewModel.Delete();
                return RedirectToAction("Index", new { sortOrder = ViewBag.SortOrder, currentFilter = ViewBag.CurrentFilter });
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Delete failed. Try again, and if the problem persists see your system administrator.");
                return View(materialDeleteViewModel);
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
