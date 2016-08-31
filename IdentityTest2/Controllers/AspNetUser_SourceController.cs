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
    public class AspNetUser_SourceController : Controller
    {
        private kapymvc1Entities db = new kapymvc1Entities();

        // GET: AspNetUser_Source
        public ActionResult Index()
        {
            var aspNetUser_Source = db.AspNetUser_Source.Include(a => a.Source).Include(a => a.AspNetUser);
            return View(aspNetUser_Source.ToList());
        }

        // GET: AspNetUser_Source/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser_Source aspNetUser_Source = db.AspNetUser_Source.Find(id);
            if (aspNetUser_Source == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser_Source);
        }

        // GET: AspNetUser_Source/Create
        public ActionResult Create()
        {
            ViewBag.souceId = new SelectList(db.Sources, "sourceId", "sourceName");
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: AspNetUser_Source/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "usersourceId,UserId,souceId,subscribeTime")] AspNetUser_Source aspNetUser_Source)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUser_Source.Add(aspNetUser_Source);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.souceId = new SelectList(db.Sources, "sourceId", "sourceName", aspNetUser_Source.souceId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUser_Source.UserId);
            return View(aspNetUser_Source);
        }

        // GET: AspNetUser_Source/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser_Source aspNetUser_Source = db.AspNetUser_Source.Find(id);
            if (aspNetUser_Source == null)
            {
                return HttpNotFound();
            }
            ViewBag.souceId = new SelectList(db.Sources, "sourceId", "sourceName", aspNetUser_Source.souceId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUser_Source.UserId);
            return View(aspNetUser_Source);
        }

        // POST: AspNetUser_Source/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "usersourceId,UserId,souceId,subscribeTime")] AspNetUser_Source aspNetUser_Source)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser_Source).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.souceId = new SelectList(db.Sources, "sourceId", "sourceName", aspNetUser_Source.souceId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUser_Source.UserId);
            return View(aspNetUser_Source);
        }

        // GET: AspNetUser_Source/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser_Source aspNetUser_Source = db.AspNetUser_Source.Find(id);
            if (aspNetUser_Source == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser_Source);
        }

        // POST: AspNetUser_Source/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetUser_Source aspNetUser_Source = db.AspNetUser_Source.Find(id);
            db.AspNetUser_Source.Remove(aspNetUser_Source);
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
