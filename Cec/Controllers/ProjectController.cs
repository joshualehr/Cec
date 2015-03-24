using Cec.Models;
using Cec.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public ProjectController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: /Project/
        public ActionResult Index()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            if (manager.IsInRole(currentUser.Id, "canAdminister") && db.Projects.Count() > 0)
            {
                return View(new ProjectIndexViewModel().ListAll());
            }
            else if (db.Projects.Where(p => p.User.Id == currentUser.Id).Count() > 0)
            {
                return View(new ProjectIndexViewModel().ListByUser(currentUser.Id));
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        // GET: /Project/Details/5
        [Authorize(Roles = "canViewDetails")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = new ProjectDetailsViewModel(id ?? Guid.Empty);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: /Project/Create
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Create([Bind(Include = "ProjectID,Designation,Description,PurchaseOrder,Address,City,State,PostalCode")] Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = manager.FindById(User.Identity.GetUserId());
                    project.User = currentUser;
                    db.Projects.Add(project);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = project.ProjectID });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(project);
        }

        // GET: /Project/Create
        [Authorize(Roles = "canAdminister")]
        public ActionResult AdminCreate()
        {
            return View();
        }

        // POST: /Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult AdminCreate([Bind(Include = "ProjectID,Designation,Description,PurchaseOrder,Address,City,State,PostalCode")] Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Projects.Add(project);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = project.ProjectID });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(project);
        }

        // GET: /Project/Edit/5
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Edit([Bind(Include = "ProjectID,Designation,Description,PurchaseOrder,Address,City,State,PostalCode")] Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = project.ProjectID });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(project);
        }

        // GET: /Project/Delete/5
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
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canAdminister")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                Project project = db.Projects.Find(id);
                db.Projects.Remove(project);
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
