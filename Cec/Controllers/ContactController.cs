using Cec.Models;
using Cec.ViewModels;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Controllers
{
    [Authorize(Roles = "isEmployee")]
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Contact/
        public ActionResult Index()
        {
            ContactIndexViewModel contactIndexViewModel = new ContactIndexViewModel();
            if (contactIndexViewModel.Contacts.Count > 0)
            {
                return View(contactIndexViewModel);
            }
            return RedirectToAction("Create");
        }

        // GET: /Contact/(ProjectId)
        public ActionResult IndexByProject(Guid id)
        {
            ViewBag.ContactId = new ContactSelectList(id);
            ContactIndexByProjectViewModel contactIndexByProjectViewModel = new ContactIndexByProjectViewModel(id);
            return View(contactIndexByProjectViewModel);
        }

        //POST: /Contact/IndexByProject(ProjectId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
        public ActionResult IndexByProject([Bind(Include = "Contacts,ProjectId,Project,ContactId")] ContactIndexByProjectViewModel contact)
        {
            try
            {
                if (ModelState.IsValid)
                    {
                        return RedirectToAction("IndexByProject", new { id = contact.AssociateContactWithProject(contact.ProjectId, contact.ContactId) });
                    }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            ViewBag.ContactId = new ContactSelectList(contact.ProjectId);
            return View(contact);
        }

        // GET: /Contact/Details/(ContactId)
        [Authorize(Roles = "canViewDetails")]
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
        [Authorize(Roles = "canEdit")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
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

        // GET: /Contact/Edit/(ContactId)
        [Authorize(Roles = "canEdit")]
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

        // POST: /Contact/Edit/(ContactId)
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit")]
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

        // POST: /Contact/Delete/(ContactId)
        [HttpPost]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                Contact contact = db.Contacts.Single(c => c.ContactID == id);
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

        // POST: /Contact/DeleteProjectContact/(ContactId)
        [HttpPost]
        [Authorize(Roles = "canDelete")]
        public ActionResult DeleteProjectContact(Guid id, Guid contactid)
        {
            try
            {
                var projectContact = db.ProjectContacts.Single(pc => pc.ProjectID == id && pc.ContactID == contactid);
                db.ProjectContacts.Remove(projectContact);
                db.SaveChanges();
                return RedirectToAction("IndexByProject", new { id = id });
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("IndexByProject", new { id = id, saveChangesError = true });
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
