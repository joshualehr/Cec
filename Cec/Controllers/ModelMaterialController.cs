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
    public class ModelMaterialController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ModelMaterial/(ModelId)
        public ActionResult Index(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var modelmaterials = new ModelMaterialIndexViewModel().ListByModel(id);
            if (modelmaterials != null)
            {
                return View(modelmaterials);
            }
            else
            {
                return RedirectToAction("Create", new { id = id });
            }
        }

        // GET: /ModelMaterial/Create/(ModelId)
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new ModelMaterialCreateViewModel(id ?? Guid.Empty));
        }

        // POST: /ModelMaterial/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,Project,ModelId,Model,ApplyToAllAreas,Materials")] ModelMaterialCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index", new { id = model.Create() });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }

        // GET: /ModelMaterial/Edit/(ModelId)?materialId=(MaterialId)
        public ActionResult Edit(Guid? id, Guid? materialId)
        {
            if (id == null | materialId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var modelMaterialEditViewModel = new ModelMaterialEditViewModel(id ?? Guid.Empty, materialId ?? Guid.Empty);
            if (modelMaterialEditViewModel == null)
            {
                return HttpNotFound();
            }
            return View(modelMaterialEditViewModel);
        }

        // POST: /ModelMaterial/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,Project,ModelID,MaterialID,Material,UnitOfMeasure,RoughQuantity,FinishQuantity,ApplyToExisting")] ModelMaterialEditViewModel modelMaterial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index", new { id = modelMaterial.Edit(modelMaterial.ApplyToExisting) });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(modelMaterial);
        }

        // POST: /ModelMaterial/Delete/(ModelId)?materialId=(MaterialId)
        [HttpPost]
        public ActionResult Delete(Guid id, Guid materialId)
        {
            try
            {
                var modelMaterial = db.ModelMaterials.Single(mm => mm.ModelID == id & mm.MaterialID == materialId);
                db.ModelMaterials.Remove(modelMaterial);
                var areaMaterials = db.AreaMaterials.Where(a => a.Area.ModelID == id & a.MaterialID == materialId);
                foreach (var areaMaterial in areaMaterials)
                {
                    db.AreaMaterials.Remove(areaMaterial);
                }
                db.SaveChanges();
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Delete failed. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction(nameof(ModelMaterialController.Index), new { id = id });
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
