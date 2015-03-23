using Cec.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Controllers
{
    public class AreaMaterialController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /AreaMaterial/5
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

        // GET: /AreaMaterial/Create/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var areaMaterial = new AreaMaterial();
            areaMaterial.Area = db.Areas.Find(id);
            areaMaterial.AreaID = areaMaterial.Area.AreaID;
            ViewBag.MaterialID = new SelectList(db.Materials, "MaterialID", "Designation");
            return View(areaMaterial);
        }

        // POST: /AreaMaterial/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
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
            catch (RetryLimitExceededException /* dex */)
            {
                
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            ViewBag.MaterialID = new SelectList(db.Materials, "MaterialID", "Designation", areamaterial.MaterialID);
            return View(areamaterial);
        }

        // GET: /AreaMaterial/Edit/5?areaId=5
        [Authorize(Roles = "canAdminister")]
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

        // POST: /AreaMaterial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
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
            catch (RetryLimitExceededException /* dex */)
            {

                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(areamaterial);
        }

        // GET: /AreaMaterial/Delete/5
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
            AreaMaterial areamaterial = db.AreaMaterials.Find(id, materialId);
            if (areamaterial == null)
            {
                return HttpNotFound();
            }
            return View(areamaterial);
        }

        // POST: /AreaMaterial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DeleteConfirmed(Guid id, Guid materialId)
        {
            try
            {
                AreaMaterial areamaterial = db.AreaMaterials.Find(id, materialId);
                db.AreaMaterials.Remove(areamaterial);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = areamaterial.AreaID });

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
