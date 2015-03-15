using Cec.Models;
using Cec.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;

namespace Cec.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> _manager;

        public ProfileController()
        {
            db = new ApplicationDbContext();
            _manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: /Profile/Details
        public ActionResult Details()
        {
            var currentUser = _manager.FindById(User.Identity.GetUserId());
            var contact = new ContactDetailsViewModel(currentUser.ContactID);
            if (contact == null)
            {
                return RedirectToAction("Edit");
            }
            return View(contact);
        }

        // GET: /Profile/Edit
        public ActionResult Edit()
        {
            var currentUser = _manager.FindById(User.Identity.GetUserId());
            var contact = new ContactEditViewModel(currentUser.ContactID);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: /Profile/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
