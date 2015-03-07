using Cec.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Contact/
        public ActionResult Index()
        {
            if (db.Contacts.Count() > 0)
            {
                return View(db.Contacts.ToList());
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        // GET: /Contact/Details/5
        [Authorize(Roles = "canViewDetails")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: /Contact/Create
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create([Bind(Include = "ContactID,FirstName,LastName,Company,Title,Trade,Phone,Email,Chat,Website")] Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                    {
                        db.Contacts.Add(contact);
                        db.SaveChanges();
                        return RedirectToAction("Details", new { id = contact.ContactID });
                    }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(contact);
        }

        // GET: /Contact/Edit/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: /Contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit([Bind(Include = "ContactID,FirstName,LastName,Company,Title,Trade,Phone,Email,Chat,Website")] Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = contact.ContactID });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(contact);
        }

        // GET: /Contact/Delete/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Delete(Guid? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: /Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                Contact contact = db.Contacts.Find(id);
                db.Contacts.Remove(contact);
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
