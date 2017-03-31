using Cec.Models;
using Cec.ViewModels;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Controllers
{
    [Authorize(Roles = "canManageProjects")]
    public class ModelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Model/(ProjectId)
        public ActionResult Index(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var modelIndexViewModel = new ModelIndexViewModel(id ?? Guid.Empty);
            if (modelIndexViewModel.Models.Count() > 0)
            {
                return View(modelIndexViewModel);
            }
            else
            {
                return RedirectToAction("Create", new { id = id });
            }
        }

        // GET: /Model/Details/(ModelId)
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new ModelDetailsViewModel(id ?? Guid.Empty));
        }

        // GET: /Model/Create/(ProjectId)
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new ModelCreateViewModel(id ?? Guid.Empty));
        }

        // POST: /Model/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ModelName,Description,ProjectId")] ModelCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = model.Create() });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }

        // GET: /Model/Edit/(ModelId)
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new ModelEditViewModel(id ?? Guid.Empty));
        }

        // POST: /Model/Edit/(ModelId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ModelId,ModelName,Description,ProjectId")] ModelEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = model.Edit() });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }

        // GET: /Model/Copy/(ModelId)
        public ActionResult Copy(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new ModelCopyViewModel(id ?? Guid.Empty));
        }

        // POST: /Model/Copy/(ModelId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy([Bind(Include = "ModelId,ModelName,Description,ProjectId")] ModelCopyViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = model.Copy() });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }

        // GET: /Model/Delete/5
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
            return View(new ModelDeleteViewModel(id ?? Guid.Empty));
        }

        // POST: /Model/Delete/(ModelId)
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "ModelId, ProjectId")] ModelDeleteViewModel model)
        {
            try
            {
                return RedirectToAction("Index", new { id = model.Delete() });
            }
            catch (RetryLimitExceededException)
            {
                return RedirectToAction("Delete", new { id = model.ModelId, saveChangesError = true });
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
