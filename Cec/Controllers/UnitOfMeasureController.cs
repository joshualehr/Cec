using Cec.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Views
{
    [Authorize(Roles = "canEdit")]
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: /UnitOfMeasure/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: /UnitOfMeasure/Edit/(UnitOfMeasureId)
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

        // POST: /UnitOfMeasure/Edit/(UnitOfMeasureId)
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // POST: /UnitOfMeasure/Delete/(UnitOfMeasureId)
        [HttpPost]
        [Authorize(Roles = "canDelete")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                UnitOfMeasure unitofmeasure = db.UnitOfMeasures.Single(u => u.UnitOfMeasureID == id);
                db.UnitOfMeasures.Remove(unitofmeasure);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Index", new { saveChangesError = true });
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
