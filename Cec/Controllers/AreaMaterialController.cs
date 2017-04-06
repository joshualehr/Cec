using Cec.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Controllers
{
    [Authorize(Roles = "isEmployee")]
    public class AreaMaterialController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /AreaMaterial/(AreaId)
        public ActionResult Index(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var areamaterials = db.AreaMaterials.Include(a => a.Area)
                                                .Include(a => a.Material)
                                                .Include(a => a.Material.UnitOfMeasure)
                                                .Where(a => a.AreaID == id)
                                                .OrderBy(a => a.Material.Designation);
            if (areamaterials.Count() > 0)
            {
                return View(areamaterials.ToList());
            }
            else
            {
                return RedirectToAction("Create", new { id = id });
            }
        }

        // GET: /AreaMaterial/Create/(AreaId)
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var areaMaterial = new AreaMaterial();
            areaMaterial.Area = db.Areas.Find(id);
            areaMaterial.AreaID = areaMaterial.Area.AreaID;
            ViewBag.MaterialID = new SelectList(db.Materials.OrderBy(m => m.Designation), "MaterialID", "Designation");
            return View(areaMaterial);
        }

        // POST: /AreaMaterial/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Create([Bind(Include = "AreaID,MaterialID,Quantity")] AreaMaterial areamaterial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.AreaMaterials.Add(areamaterial);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = areamaterial.AreaID });
                }
            }
            catch (RetryLimitExceededException)
            {
                
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            ViewBag.MaterialID = new SelectList(db.Materials, "MaterialID", "Designation", areamaterial.MaterialID);
            return View(areamaterial);
        }

        // GET: /AreaMaterial/Edit/(AreaId)?materialid=(MaterialId)
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Edit(Guid? id, Guid? materialId)
        {
            if (id == null | materialId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaMaterial areamaterial = db.AreaMaterials.Find(id, materialId);
            if (areamaterial == null)
            {
                return HttpNotFound();
            }
            return View(areamaterial);
        }

        // POST: /AreaMaterial/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Edit([Bind(Include = "AreaID,MaterialID,Quantity")] AreaMaterial areamaterial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(areamaterial).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = areamaterial.AreaID });
                }
            }
            catch (RetryLimitExceededException)
            {

                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(areamaterial);
        }

        // GET: /AreaMaterial/Delete/5
        [Authorize(Roles = "canManageProjects")]
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
            AreaMaterial areamaterial = db.AreaMaterials.Find(id, materialId);
            if (areamaterial == null)
            {
                return HttpNotFound();
            }
            return View(areamaterial);
        }

        // POST: /AreaMaterial/Delete/(AreaId)?materialid=(MaterialId)
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult DeleteConfirmed(Guid id, Guid materialId)
        {
            try
            {
                AreaMaterial areamaterial = db.AreaMaterials.Find(id, materialId);
                db.AreaMaterials.Remove(areamaterial);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = areamaterial.AreaID });

            }
            catch (RetryLimitExceededException)
            {
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
