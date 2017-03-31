using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cec.Models;

namespace Cec.Controllers
{
    [Authorize(Roles = "isEmployee")]
    public class ToDoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ToDo/Index/(AreaId)
        public ActionResult Index(Guid id)
        {
            List<ToDo> toDos = db.ToDos.Include(t => t.Area)
                .Include(t => t.ParentToDo)
                .Where(t => t.AreaID == id)
                .OrderBy(t => t.Heading)
                .ToList();
            if (toDos.Count > 0)
            {
                return View(toDos);
            }
            return RedirectToAction("Create", new { id = id });
        }

        // GET: ToDo/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // GET: ToDo/Create/(AreaId)
        public ActionResult Create(Guid id)
        {
            return View(new ToDo { AreaID = id });
        }

        // POST: ToDo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ToDoID,AreaID,ParentToDoID,Heading,Description,StartOn,Completed,CompletedOn,ListOrder")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                toDo.ToDoID = Guid.NewGuid();
                db.ToDos.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = toDo.AreaID });
            }
            return View(toDo);
        }

        // GET: ToDo/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaID = new SelectList(db.Areas, "AreaID", "Designation", toDo.AreaID);
            ViewBag.ParentToDoID = new SelectList(db.ToDos, "ToDoID", "Heading", toDo.ParentToDoID);
            return View(toDo);
        }

        // POST: ToDo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ToDoID,AreaID,ParentToDoID,Heading,Description,StartOn,Completed,CompletedOn,ListOrder")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaID = new SelectList(db.Areas, "AreaID", "Designation", toDo.AreaID);
            ViewBag.ParentToDoID = new SelectList(db.ToDos, "ToDoID", "Heading", toDo.ParentToDoID);
            return View(toDo);
        }

        // GET: ToDo/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // POST: ToDo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ToDo toDo = db.ToDos.Find(id);
            db.ToDos.Remove(toDo);
            db.SaveChanges();
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
