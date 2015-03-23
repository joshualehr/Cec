using Cec.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Views
{
    public class UnitOfMeasureController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /UnitOfMeasure/
        public ActionResult Index()
        {
            if (db.UnitOfMeasures.Count() > 0)
            {
                return View(db.UnitOfMeasures.ToList().OrderBy(p => p.Designation));
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        // GET: /UnitOfMeasure/Create
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /UnitOfMeasure/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
        public ActionResult Create([Bind(Include = "UnitOfMeasureID,Designation")] UnitOfMeasure unitofmeasure)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.UnitOfMeasures.Add(unitofmeasure);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(unitofmeasure);
        }

        // GET: /UnitOfMeasure/Edit/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitOfMeasure unitofmeasure = db.UnitOfMeasures.Find(id);
            if (unitofmeasure == null)
            {
                return HttpNotFound();
            }
            return View(unitofmeasure);
        }

        // POST: /UnitOfMeasure/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit([Bind(Include = "UnitOfMeasureID,Designation")] UnitOfMeasure unitofmeasure)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(unitofmeasure).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(unitofmeasure);
        }

        // GET: /UnitOfMeasure/Delete/5
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
            UnitOfMeasure unitofmeasure = db.UnitOfMeasures.Find(id);
            if (unitofmeasure == null)
            {
                return HttpNotFound();
            }
            return View(unitofmeasure);
        }

        // POST: /UnitOfMeasure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                UnitOfMeasure unitofmeasure = db.UnitOfMeasures.Find(id);
                db.UnitOfMeasures.Remove(unitofmeasure);
                db.SaveChanges();
                return RedirectToAction("Index");

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
