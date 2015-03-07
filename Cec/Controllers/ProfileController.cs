using Cec.Models;
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
        private UserManager<ApplicationUser> manager;

        public ProfileController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        // GET: /Profile/Details
        public ActionResult Details()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            Contact contact = db.Contacts.Find(currentUser.Contact.ContactID);
            if (contact == null)
            {
                return RedirectToAction("Edit");
            }
            return View(contact);
        }

        // GET: /Profile/Edit
        public ActionResult Edit()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            Contact contact = db.Contacts.Find(currentUser.Contact.ContactID);
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
        public ActionResult Edit([Bind(Include="ContactID,FirstName,LastName,Company,Title,Trade,Phone,Email,Chat,Website")] Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details");
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
