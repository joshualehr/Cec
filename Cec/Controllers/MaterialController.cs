using Cec.Helpers;
using Cec.Models;
using Cec.ViewModels;
using Microsoft.Azure.Search;
using PagedList;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Cec.Controllers
{
    [Authorize(Roles = "isEmployee")]
    public class MaterialController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ImageHandler photoHandler = new ImageHandler();
        private string relativeDirectoryPath = "/Content/Images/";

        // GET: /Material/
        // paging: https://github.com/TroyGoode/PagedList
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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

        // GET: /Material/Details/(MaterialId)
        [Authorize(Roles = "canViewDetails")]
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
        [Authorize(Roles = "canEdit")]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
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

        // GET: /Material/Edit/(MaterialId)
        [Authorize(Roles = "canEdit")]
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
        [Authorize(Roles = "canEdit")]
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

        // GET: /Material/Copy/(MaterialId)
        [Authorize(Roles = "canEdit")]
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

        // POST: /Material/Copy/(MaterialId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
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

        // POST: /Material/Delete/(MaterialId)
        [HttpPost]
        [Authorize(Roles = "canDelete")]
        public ActionResult Delete(Guid id, int? page, string sortOrder, string currentFilter)
        {
            ViewBag.Page = page;
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentFilter = currentFilter;
            try
            {
                Material material = db.Materials.Single(m => m.MaterialID == id);
                if (!photoHandler.OtherMaterialUsesImage(material.ImagePath)) //Image not used elsewhere
                {
                    photoHandler.DeleteImage(Server.MapPath(material.ImagePath));
                }
                ViewBag.Page = page;
                ViewBag.SortOrder = sortOrder;
                ViewBag.CurrentFilter = currentFilter;
                db.Materials.Remove(material);
                db.SaveChanges();
                return RedirectToAction("Index", new { page = page, sortOrder = sortOrder, currentFilter = currentFilter });
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Delete failed. Try again, and if the problem persists see your system administrator.");
                return RedirectToAction("Details", new { id = id, page = page, sortOrder = sortOrder, currentFilter = currentFilter });
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
