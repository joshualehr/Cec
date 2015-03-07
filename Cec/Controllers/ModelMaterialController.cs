using Cec.Helpers;
using Cec.Models;
using Cec.ViewModels;
using PagedList;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CompleteElectric.Controllers
{
    public class ModelMaterialController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ModelMaterial/5
        public ActionResult Index(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var modelmaterials = db.ModelMaterials.Include(m => m.Model)
                                                  .Include(m => m.Material)
                                                  .Include(m => m.Material.UnitOfMeasure)
                                                  .Where(m => m.ModelID == id)
                                                  .OrderBy(m => m.Material.Designation);
            if (modelmaterials.Count() > 0)
            {
                return View(modelmaterials.ToList());
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
        public ActionResult Create([Bind(Include = "ModelId, ApplyToAllAreas, OnlyProjectMaterial, Materials")] ModelMaterialCreateViewModel model)
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
            ModelMaterial modelmaterial = db.ModelMaterials.Find(id, materialId);
            if (modelmaterial == null)
            {
                return HttpNotFound();
            }
            return View(modelmaterial);
        }

        // POST: /ModelMaterial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit([Bind(Include = "ModelID,MaterialID,Quantity")] ModelMaterial modelmaterial, bool applyToAll = false)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(modelmaterial).State = EntityState.Modified;
                    if (applyToAll)
                    {
                        var areaMaterials = db.AreaMaterials.Where(a => a.Area.ModelID == modelmaterial.ModelID && a.MaterialID == modelmaterial.MaterialID);
                        foreach (var item in areaMaterials)
                        {
                            item.Quantity = modelmaterial.Quantity;
                            db.Entry(item).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = modelmaterial.ModelID });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(modelmaterial);
        }

        // GET: /ModelMaterial/Delete/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Delete(Guid? id, Guid? materialId, bool? saveChangesError = false)
        {
            if (id == null | materialId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            ModelMaterial modelmaterial = db.ModelMaterials.Find(id, materialId);
            if (modelmaterial == null)
            {
                return HttpNotFound();
            }
            return View(modelmaterial);
        }

        // POST: /ModelMaterial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DeleteConfirmed(Guid id, Guid materialId, bool applyToAll = false)
        {
            try
            {
                ModelMaterial modelmaterial = db.ModelMaterials.Find(id, materialId);
                db.ModelMaterials.Remove(modelmaterial);
                if (applyToAll)
                {
                    var areaMaterials = db.AreaMaterials.Where(a => a.Area.ModelID == modelmaterial.ModelID & a.MaterialID == modelmaterial.MaterialID);
                    foreach (var item in areaMaterials)
                    {
                        db.AreaMaterials.Remove(item);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { id = modelmaterial.ModelID });

            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
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
