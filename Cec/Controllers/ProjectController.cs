using Cec.Models;
using Cec.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Cec.Controllers
{
    [Authorize(Roles = "isEmployee")]
    public class ProjectController : Controller
    {
        private ApplicationDbContext db;

        public ProjectController()
        {
            db = new ApplicationDbContext();
        }

        // GET: /Project/
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            List<ProjectIndexViewModel> projects = null;

            if (User.IsInRole("canAdminister"))
            {
                projects = new ProjectIndexViewModel().ListAll();
            }
            else if (User.IsInRole("canManageProjects"))
            {
                projects = new ProjectIndexViewModel().ListByManager(userId);
            }
            else if (User.IsInRole("isEmployee"))
            {
                projects = new ProjectIndexViewModel().ListByEmployee(userId);
            }

            if (projects.Count > 0)
            {
                return View(projects);
            }
            return RedirectToAction("Create");
        }

        // GET: /Project/Details/(ProjectId)
        [Authorize(Roles = "canViewDetails")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new ProjectDetailsViewModel(id ?? Guid.Empty));
        }

        // GET: /Project/Create
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Create()
        {
            return View(new ProjectCreateViewModel());
        }

        // POST: /Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Create([Bind(Include = "Designation,Description,PurchaseOrder,Address,City,State,PostalCode,UserId")] ProjectCreateViewModel project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    project.UserId = project.UserId ?? User.Identity.GetUserId();
                    return RedirectToAction("Details", new { id = project.Create() });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(project);
        }

        // GET: /Project/Edit/(ProjectId)
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = new ProjectEditViewModel(id ?? Guid.Empty);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Project/Edit/(ProjectId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canManageProjects")]
        public ActionResult Edit([Bind(Include = "ProjectId,Designation,Description,PurchaseOrder,Address,City,State,PostalCode,UserId")] ProjectEditViewModel project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Details", new { id = project.Edit() });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(project);
        }

        // POST: /Project/Delete/(ProjectId)
        [HttpPost]
        [Authorize(Roles = "canAdminister")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                db.Projects.Remove(db.Projects.Single(p => p.ProjectID == id));
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Delete failed. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction("Index");
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
