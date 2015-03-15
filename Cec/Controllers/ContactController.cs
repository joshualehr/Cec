using Cec.Models;
using Cec.ViewModels;
using System;
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
            var contacts = new ContactIndexViewModel().ListAll();
            if (contacts.Count() > 0)
            {
                return View(contacts);
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        // GET: /Contact/Details/5
        [Authorize(Roles = "canAdminister, isEmployee")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contactId = id ?? Guid.Empty;
            var contact = new ContactDetailsViewModel(contactId);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: /Contact/Create
        [Authorize(Roles = "canAdminister, isEmployee")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister, isEmployee")]
        public ActionResult Create([Bind(Include = "ContactID,FirstName,LastName,Company,Title,Trade,Phone,Email,Chat,Website")] ContactCreateViewModel contact)
        {
            try
            {
                if (ModelState.IsValid)
                    {
                        return RedirectToAction("Details", new { id = contact.CreateContact() });
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
        [Authorize(Roles = "canAdminister, isEmployee")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contactId = id ?? Guid.Empty;
            var contact = new ContactEditViewModel(contactId);
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
        [Authorize(Roles = "canAdminister, isEmployee")]
        public ActionResult Edit([Bind(Include = "ContactID,FirstName,LastName,Company,Title,Trade,Phone,Email,Chat,Website")] ContactEditViewModel contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = contact.EditContact() });
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
            var contactId = id ?? Guid.Empty;
            var contact = new ContactDeleteViewModel(contactId);
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
                new ContactDeleteViewModel(id).DeleteContact();
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
