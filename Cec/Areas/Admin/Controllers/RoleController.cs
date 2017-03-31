using Cec.Helpers;
using Cec.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace Cec.Areas.Admin.Controllers
{
    [Authorize(Roles = "canAdminister")]
    [SelectedTab("roles")]
    public class RoleController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public RoleController() : this(new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db))) { }

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            RoleManager = roleManager;
        }

        public RoleManager<IdentityRole> RoleManager { get; private set; }

        // GET: Admin/Role/Index
        public ActionResult Index()
        {
            return View(RoleManager.Roles.ToList());
        }

        //Get: /Admin/Role/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: /Admin/Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!RoleManager.RoleExists(model.Name))
                    {
                        IdentityResult result = RoleManager.Create(new IdentityRole(model.Name));
                    }
                    return RedirectToAction("index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(model);
        }

        // POST: /Admin/Role/Delete/(RoleId)
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                RoleManager.Delete(RoleManager.FindById(id));
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