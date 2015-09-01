using Cec.Areas.Admin.ViewModels;
using Cec.Helpers;
using Cec.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cec.Areas.Admin.Controllers
{
    [Authorize(Roles="canAdminister")]
    [SelectedTab("users")]
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            return View(new UserIndexViewModel());
        }

        //Get: /Admin/Create
        public ActionResult Create()
        {
            return View(new UserCreateViewModel());
        }

        //POST: /Admin/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(UserCreateViewModel form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index", new { id = form.Create() });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(form);
        }

        public ActionResult Edit(string id)
        {
            var user = db.AspNetUsers.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(new UserEdit
            {
                UserName = user.UserName,
                Roles = db.AspNetRoles.Select(role => new RoleCheckBox
                {
                    Id = role.Id,
                    IsChecked = user.AspNetRoles.Contains(role),
                    Name = role.Name
                }).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(string id, UserEdit form)
        {
            var user = new User(id);
            if (user == null)
                return HttpNotFound();

            SyncRoles(form.Roles, user.Roles);

            if (db.AspNetUsers.Any(u => u.UserName == form.UserName && u.Id != id))
                ModelState.AddModelError("UserName", "UserName must be unique.");

            if (!ModelState.IsValid)
                return View(form);

            user.UserName = form.UserName;

            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("index");
        }

        public ActionResult ResetPassword(string id)
        {
            var user = new User(id);
            if (user == null)
                return HttpNotFound();

            return View(new UserResetPasswordViewModel
            {
                UserName = user.UserName
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string id, UserResetPasswordViewModel form)
        {
            var user = new User(id);
            if (user == null)
                return HttpNotFound();
            form.UserName = user.UserName;
            if (!ModelState.IsValid)
                return View(form);
            user.SetPassword(form.Password);
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            
            return RedirectToAction("index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            var user = new AspNetUser(id);
            if (user == null)
                return HttpNotFound();
            db.AspNetUsers.Remove(user);
            db.SaveChanges();

            return RedirectToAction("index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var db = new ApplicationDbContext();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}