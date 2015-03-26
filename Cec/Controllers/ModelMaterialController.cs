using Cec.Models;
using Cec.ViewModels;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Controllers
{
    public class ModelMaterialController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ModelMaterial/5
        [Authorize(Roles = "canAdminister")]
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

        // GET: /ModelMaterial/Create/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new ModelMaterialCreateViewModel(id ?? Guid.Empty));
        }

        // POST: /ModelMaterial/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create([Bind(Include = "ProjectId,Project,ModelId,Model,ApplyToAllAreas,Materials")] ModelMaterialCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index", new { id = model.Create() });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }

        // GET: /ModelMaterial/Edit/5?materialId=5
        [Authorize(Roles = "canAdminister")]
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

        // POST: /ModelMaterial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit([Bind(Include = "ProjectId,Project,ModelID,MaterialID,Material,ImagePath,UnitOfMeasure,Quantity,ApplyToExisting")] ModelMaterialEditViewModel modelMaterial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index", new { id = modelMaterial.Edit(modelMaterial.ApplyToExisting) });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(modelMaterial);
        }

        // GET: /ModelMaterial/Delete/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Delete(Guid? id, Guid? materialId)
        {
            if (id == null | materialId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var modelMaterialDeleteViewModel = new ModelMaterialDeleteViewModel(id ?? Guid.Empty, materialId ?? Guid.Empty);
            if (modelMaterialDeleteViewModel == null)
            {
                return HttpNotFound();
            }
            return View(modelMaterialDeleteViewModel);
        }

        // POST: /ModelMaterial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DeleteConfirmed([Bind(Include = "ProjectId,Project,ModelID,MaterialID,Material,ApplyToExisting")] ModelMaterialDeleteViewModel modelMaterialDeleteViewModel)
        {
            try
            {
                return RedirectToAction("Index", new { id = modelMaterialDeleteViewModel.Delete() });
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Delete failed. Try again, and if the problem persists see your system administrator.");
                return View(modelMaterialDeleteViewModel);
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
