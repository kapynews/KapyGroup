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
    public class News1Controller : Controller
    {
        private kapymvc1Entities db = new kapymvc1Entities();

        // GET: News1
        // GET: News1
        public ActionResult Index(string sortOrder)
        {
            ViewBag.DateSortParm = sortOrder == "ID" ? "Time" : "ID";
            var news = from s in db.News1
                       select s;
            switch (sortOrder)
            {

                case "ID":
                    news = news.OrderBy(s => s.newsId);
                    break;
                case "Time":
                    news = news.OrderByDescending(s => s.newsTime);
                    break;
                default:
                    news = news.OrderByDescending(s => s.newsTime).OrderByDescending(s => s.newsDate);
                    break;
            }
            //var news1 = db.News1.Include(n => n.Category).Include(n => n.Source);
            return View(news.ToList());
        }

        // GET: News1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News1 news1 = db.News1.Find(id);
            if (news1 == null)
            {
                return HttpNotFound();
            }
            return View(news1);
        }

        // GET: News1/Create
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName");
            ViewBag.sourceId = new SelectList(db.Sources, "sourceId", "sourceName");
            return View();
        }

        // POST: News1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "newsId,uniqueTitle,newsTitle,newsDate,newsTime,author,sourceId,categoryId,origUrl,picUrl,newsContent,numOfClicks,numOfLikes")] News1 news1)
        {
            if (ModelState.IsValid)
            {
                db.News1.Add(news1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", news1.categoryId);
            ViewBag.sourceId = new SelectList(db.Sources, "sourceId", "sourceName", news1.sourceId);
            return View(news1);
        }

        // GET: News1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News1 news1 = db.News1.Find(id);
            if (news1 == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", news1.categoryId);
            ViewBag.sourceId = new SelectList(db.Sources, "sourceId", "sourceName", news1.sourceId);
            return View(news1);
        }

        // POST: News1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "newsId,uniqueTitle,newsTitle,newsDate,newsTime,author,sourceId,categoryId,origUrl,picUrl,newsContent,numOfClicks,numOfLikes")] News1 news1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", news1.categoryId);
            ViewBag.sourceId = new SelectList(db.Sources, "sourceId", "sourceName", news1.sourceId);
            return View(news1);
        }

        // GET: News1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News1 news1 = db.News1.Find(id);
            if (news1 == null)
            {
                return HttpNotFound();
            }
            return View(news1);
        }

        // POST: News1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News1 news1 = db.News1.Find(id);
            db.News1.Remove(news1);
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


        public ActionResult _MenuView()
        {
            return PartialView("_MenuView", db.Categories.ToList());
        }

        // GET: News1/Category/1
        public ActionResult Category(int? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categoryModel = db.Categories.Include("News1")
                .Single(n => n.categoryId == categoryId);
            return View(categoryModel);
        }
    }
}
