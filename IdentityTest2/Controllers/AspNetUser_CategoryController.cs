using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentityTest2.Models;

namespace IdentityTest2.Controllers
{
    public class AspNetUser_CategoryController : Controller
    {
        private kapymvc1Entities db = new kapymvc1Entities();

        // GET: AspNetUser_Category
        public ActionResult Index()
        {
            var aspNetUser_Category = db.AspNetUser_Category.Include(a => a.AspNetUser).Include(a => a.Category);
            return View(aspNetUser_Category.ToList());
        }

        // GET: AspNetUser_Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser_Category aspNetUser_Category = db.AspNetUser_Category.Find(id);
            if (aspNetUser_Category == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser_Category);
        }

        // GET: AspNetUser_Category/Create
        public ActionResult Create()
        {
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName");
            return View();
        }

        // POST: AspNetUser_Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "usercategoryId,userId,categoryId")] AspNetUser_Category aspNetUser_Category)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUser_Category.Add(aspNetUser_Category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUser_Category.userId);
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", aspNetUser_Category.categoryId);
            return View(aspNetUser_Category);
        }

        // GET: AspNetUser_Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser_Category aspNetUser_Category = db.AspNetUser_Category.Find(id);
            if (aspNetUser_Category == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUser_Category.userId);
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", aspNetUser_Category.categoryId);
            return View(aspNetUser_Category);
        }

        // POST: AspNetUser_Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "usercategoryId,userId,categoryId")] AspNetUser_Category aspNetUser_Category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUser_Category.userId);
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", aspNetUser_Category.categoryId);
            return View(aspNetUser_Category);
        }

        // GET: AspNetUser_Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser_Category aspNetUser_Category = db.AspNetUser_Category.Find(id);
            if (aspNetUser_Category == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser_Category);
        }

        // POST: AspNetUser_Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetUser_Category aspNetUser_Category = db.AspNetUser_Category.Find(id);
            db.AspNetUser_Category.Remove(aspNetUser_Category);
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
