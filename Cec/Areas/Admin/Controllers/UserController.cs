using Cec.Areas.Admin.ViewModels;
using Cec.Helpers;
using Cec.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace Cec.Areas.Admin.Controllers
{
    [Authorize(Roles="canAdminister")]
    [SelectedTab("users")]
    public class UserController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public UserController() : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db)), new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db)), new DpapiDataProtectionProvider("CecWeb")) { }

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, DpapiDataProtectionProvider provider)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            Provider = provider;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }
        public DpapiDataProtectionProvider Provider { get; private set; }

        //
        // GET: /Admin/User/Index
        public ActionResult Index()
        {
            foreach (ApplicationUser user in UserManager.Users.ToList())
            {
                user.AllRoles = string.Join(", ", UserManager.GetRoles(user.Id));
            }
            return View(new UserIndexViewModel() { Users = UserManager.Users });
        }

        //Get: /Admin/User/Create
        public ActionResult Create()
        {
            return View(new UserCreateViewModel {
                Roles = RoleManager.Roles.Select(role => new RoleCheckBox {
                    Id = role.Id,
                    IsChecked = false,
                    Name = role.Name
                }).ToList()
            });
        }

        //POST: /Admin/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName, Password, Roles")] UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Contact contact = new Contact {
                        FirstName = model.UserName
                    };
                    db.Contacts.Add(contact);
                    db.SaveChanges();

                    ApplicationUser user = new ApplicationUser {
                        UserName = model.UserName, 
                        ContactID = contact.ContactID
                    };

                    IdentityResult userResult = UserManager.Create(user, model.Password);
                    if (userResult.Succeeded)
                    {
                        foreach (var item in model.Roles.Where(r => r.IsChecked == true))
                        {
                            UserManager.AddToRole(user.Id, RoleManager.FindById(item.Id).Name);
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrors(userResult);
                        return View(model);
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(model);
        }

        // GET: /Admin/User/Edit/(UserId)
        public ActionResult Edit(string id)
        {
            ApplicationUser user = UserManager.FindById(id);
            List<RoleCheckBox> roles = new List<RoleCheckBox>();
            foreach (IdentityRole role in RoleManager.Roles.ToList())
            {
                roles.Add(new RoleCheckBox {
                    Id = role.Id,
                    Name = role.Name,
                    IsChecked = UserManager.IsInRole(user.Id, role.Name)
                });
            }

            return View(new UserEditViewModel {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles
            });
        }

        // POST: /Admin/User/Edit/(UserId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId, UserName, Roles")]UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    ApplicationUser user = UserManager.FindById(model.UserId);
                    user.UserName = model.UserName;
                    IdentityResult result = UserManager.Update(user);
                    if (result.Succeeded)
                    {
                        foreach (var checkbox in model.Roles)
                        {
                            if (checkbox.IsChecked)
                            {
                                if (!UserManager.IsInRole(user.Id, checkbox.Name))
                                {
                                    UserManager.AddToRole(user.Id, checkbox.Name);
                                }
                            }
                            else
                            {
                                if (UserManager.IsInRole(user.Id, checkbox.Name))
                                {
                                    UserManager.RemoveFromRole(user.Id, checkbox.Name);
                                }
                            }
                        }
                        return RedirectToAction("index");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(model);
        }

        // GET: /Admin/User/ResetPassword/(UserId)
        public ActionResult ResetPassword(string id)
        {
            var user = UserManager.FindById(id);
            return View(new UserResetPasswordViewModel {
                UserId = user.Id,
                Username = user.UserName
            });
        }

        // POST: /Admin/User/ResetPassword/(UserId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(UserResetPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(Provider.Create("ResetPasswordConfirmation"));
                    var user = UserManager.FindById(model.UserId);
                    var token = UserManager.GeneratePasswordResetToken(user.Id);
                    var result = UserManager.ResetPassword(user.Id, token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("index");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }

        // POST: /Admin/User/Delete/(UserId)
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                ApplicationUser applicationUser = UserManager.FindById(id);
                Contact contact = db.Contacts.SingleOrDefault(c => c.ContactID == applicationUser.ContactID);
                IdentityResult result = UserManager.Delete(applicationUser);
                if (result.Succeeded)
                {
                    if (contact != null)
                    {
                        db.Contacts.Remove(contact);
                        db.SaveChanges();
                    }
                    return RedirectToAction("index");
                }
                else
                {
                    AddErrors(result);
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
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

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion
    }
}