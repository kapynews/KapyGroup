using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentityTest2.Models;
using Microsoft.AspNet.Identity;
using System.Text;

namespace IdentityTest2.Controllers
{
    public class Comment1Controller : Controller
    {
        private kapymvc1Entities db = new kapymvc1Entities();

        // GET: Comment1
        public ActionResult Index()
        {
            var comment1 = db.Comment1.Include(c => c.AspNetUser).Include(c => c.News1);
            return View(comment1.ToList());
        }

        // GET: Comment1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment1 comment1 = db.Comment1.Find(id);
            if (comment1 == null)
            {
                return HttpNotFound();
            }
            return View(comment1);
        }

        // GET: Comment1/Create
        public ActionResult Create()
        {
            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.newsId = new SelectList(db.News1, "newsId", "uniqueTitle");
            return View();
        }

        // POST: Comment1/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "commentId,userId,newsId,postTime,commentContent,isDisplayed,numOfComentLikes")] Comment1 comment1)
        {
            if (ModelState.IsValid)
            {
                db.Comment1.Add(comment1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", comment1.userId);
            ViewBag.newsId = new SelectList(db.News1, "newsId", "uniqueTitle", comment1.newsId);
            return View(comment1);
        }

        // GET: Comment1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment1 comment1 = db.Comment1.Find(id);
            if (comment1 == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = User.Identity.GetUserId<int>();
            //ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", comment1.userId);
            //ViewBag.newsId = new SelectList(db.News1, "newsId", "uniqueTitle", comment1.newsId);
            ViewBag.newsId = comment1.newsId;
            return View(comment1);
        }

        // POST: Comment1/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "commentId,userId,newsId,postTime,commentContent,isDisplayed,numOfComentLikes")] Comment1 comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "News1", new { id = comment.newsId });
            }

            ViewBag.userId = comment.userId;
            ViewBag.newsId = comment.newsId;
            //ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", comment1.userId);
            //ViewBag.newsId = new SelectList(db.News1, "newsId", "uniqueTitle", comment1.newsId);
            return View(comment);
        }

        // GET: Comment1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment1 comment1 = db.Comment1.Find(id);
            if (comment1 == null)
            {
                return HttpNotFound();
            }
            ViewBag.newsID = comment1.newsId;
            return View(comment1);
        }

        // POST: Comment1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment1 comment = db.Comment1.Find(id);
            db.Comment1.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "News1", new { id = comment.newsId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //This function retrieve all comments for a given news
        // GET: Comments/CommentForNews/5
        public ActionResult CommentForNews(int? id, string sortOrder)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = db.News1.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }


            var commentNews = db.Comment1.Include(c => c.AspNetUser).Include(c => c.News1).Where(x => x.newsId == id);
            var newsTitle = db.News1.Single(x => x.newsId == id).newsTitle;
            ViewBag.newsTitle = newsTitle;
            ViewBag.newsID = id;

            //The comments will be sorted by the number of likes
            ViewBag.DateSortParm = sortOrder == "ID" ? "Time" : "ID";
            switch (sortOrder)
            {

                case "ID":
                    commentNews = commentNews.OrderBy(c => c.commentId);
                    break;
                case "Time":
                    commentNews = commentNews.OrderByDescending(c => c.postTime);
                    break;
                default:
                    commentNews = commentNews.OrderByDescending(c => c.numOfComentLikes);
                    break;
            }

            return View(commentNews.ToList());
        }

        // GET: Comments/CreateCommentForNews/5
        public ActionResult CreateCommentForNews(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = db.News1.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }

            ViewBag.userId = User.Identity.GetUserId<int>();
            ViewBag.newsId = id;
            ViewBag.message = "ADD A COMMENT";
            //ViewBag.userId = new SelectList(db.AspNetUsers, "userId", "userName");
            //ViewBag.newsId = new SelectList(db.News1, "newsId", "newsId");
            return View();
        }

        // POST: Comments/CreateCommentForNews/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCommentForNews(int id, [Bind(Include = "commentId,userId,newsId,postTime,commentContent,isDisplayed,numOfComentLikes")] Comment1 comment)
        {
            var user_id = User.Identity.GetUserId<int>();
            if (user_id == 0)
            {
                ViewBag.message = "Sorry, please login or register First.";
                String returnURL = "Comments/CreateCommentForNews/" + id;
                return RedirectToAction("Login", "Account", "returnURL");
            }

            if (ModelState.IsValid)
            {
                comment.numOfComentLikes = 0;
                ViewBag.message = "ADD A COMMENT";
                comment.newsId = id;
                comment.userId = User.Identity.GetUserId<int>();
                db.Comment1.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "News1", new { id = comment.newsId });
            }

            ViewBag.userId = User.Identity.GetUserId<int>();
            ViewBag.newsId = comment.newsId;
            //ViewBag.userId = new SelectList(db.Users, "userId", "userName", comment.userId);
            //ViewBag.newsId = new SelectList(db.News1, "newsId", "uniqueTitle", comment.newsId);

            return View(comment);
        }

        // GET: Comments/Like/5

        public ActionResult Like(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment1 comment = db.Comment1.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.commentID = id;
            return PartialView(comment);
        }




        //LikeComment increases the numberOfLikes for a given comment
        // POST: Comments/LikeAComment/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost, ActionName("Like")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LikeAComment(int id)
        {
            Comment1 comment = db.Comment1.Find(id);
            comment.numOfComentLikes++;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", "News1", new { id = comment.newsId });
        }
    }
}