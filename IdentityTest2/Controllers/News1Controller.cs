using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentityTest2.Models;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Data.SqlClient;

namespace IdentityTest2.Controllers
{
    public class News1Controller : Controller
    {
        private kapymvc1Entities db = new kapymvc1Entities();

        // GET: News1
        // GET: News1
        [AllowAnonymous]
        public ActionResult Index(string sortOrder, int? page)
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
            return View(news.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: News1/Details/5
        [AllowAnonymous]
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
            
            int user_id = User.Identity.GetUserId<int>();
            if (user_id != 0) {
                IEnumerable<AspNetUser_Source> aspNetUser_Sources = db.AspNetUser_Source.Where(n => n.UserId == user_id);
                foreach (var row in aspNetUser_Sources)
                {
                    if (news1.Source.sourceId == row.souceId)
                    {
                        ViewBag.Message = "subscribed already";
                    }
                }
            }
            return View(news1);
        }

        [Authorize(Roles = "admin")]
        public ActionResult NewsManagement(string sortOrder, int? page)
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
            return View(news.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: News1/Create
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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

        [AllowAnonymous]
        public ActionResult _MenuView()
        {
            return PartialView("_MenuView", db.Categories.ToList());
        }
        //public ActionResult _CategoryList()
        //{
        //    return PartialView("_CategoryList", db.Categories.ToList());
        //}


        // GET: News1/Category/1
        [AllowAnonymous]
        public ActionResult Category(int? categoryId, int? page)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var categoryModel = db.Categories.Include("News1")
                .Single(n => n.categoryId == categoryId);
            ViewBag.Message = categoryModel.categoryName;
            ViewBag.categoryId = categoryId;
            //news.OrderByDescending(s => s.newsTime).OrderByDescending(s => s.newsDate)
            IEnumerable <IdentityTest2.Models.News1> newsList = categoryModel.News1.ToList().OrderByDescending(s => s.crawlTime);
            return View(newsList.ToPagedList(page ?? 1, 5));

    }
        //// GET: News1/Recommend
        //[Authorize]
        //public ActionResult Recommend()
        //{
        //    int userId = User.Identity.GetUserId<int>();
        //    if (userId == 0)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    //Get students enrolled in a particular course
        //    //dc.Students.Where(s => s.StudentCourseEnrollments.Any(e => e.CourseID == courseID)
        //    var selectedCategoriesModel = db.Categories.Include("News1").Where(c => c.AspNetUser_Category.Any(u => u.userId == userId));
        //    IEnumerable<Category> selectCategories = selectedCategoriesModel.ToList();
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("Recommended News for you in categories: " + "\n");
        //    foreach (var c in selectCategories)
        //    {
        //        sb.Append(c.categoryName + " ");
        //    }
        //    //sb.Remove(sb.ToString().LastIndexOf(","), 1);
        //    ViewBag.message = sb.ToString();
        //    return View(selectedCategoriesModel);
        //}

        // GET: News1/RecommendToYou()
        //Needs modification
        [Authorize]
        public ActionResult RecommendToYou(int? page)
        {
            int userId = User.Identity.GetUserId<int>();
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            StringBuilder sb = new StringBuilder();
            StringBuilder message1 = new StringBuilder();
            message1.Append("1");
            IEnumerable<AspNetUser_Category> selectCategories = db.AspNetUser_Category.Where(n => n.userId == userId);
            List<int> categoryIds = new List<int>();
            sb.Append("Recommended news for you in categories: ");
            foreach (var c in selectCategories)
            {
                message1.Append(c.Category.categoryName + "  ");
                sb.Append(c.Category.categoryName + "   ");
                categoryIds.Add(c.Category.categoryId);
            }
            if (message1.ToString() == "1")
            {
                return RedirectToAction("Insert", "AspNetUser_Category");
            }
            else {
                StringBuilder message2 = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                message2.Append("1");
                IEnumerable<AspNetUser_Source> selectSources = db.AspNetUser_Source.Where(n => n.UserId == userId);
                List<int> sourceIds = new List<int>();
                sb2.Append(sb);
                sb2.Append("\n\nand in sources: " + "\n");
                foreach (var c in selectSources)
                {
                    message2.Append(c.Source.sourceName + "  ");
                    sb2.Append(c.Source.sourceName + "   ");
                    sourceIds.Add(c.Source.sourceId);
                }
                if (message2.ToString() == "1")
                {
                    var newsModel = db.News1.Where(n => categoryIds.Contains(n.categoryId));
                    ViewBag.message = sb.ToString();
                    IEnumerable<IdentityTest2.Models.News1> newsList = newsModel.ToList().OrderByDescending(s => s.crawlTime);
                    return View(newsList.ToPagedList(page ?? 1, 5));
                }
                else {

                    ViewBag.message = sb2.ToString();
                    var newsModel = db.News1.Where(n => categoryIds.Contains(n.categoryId)).Where(n => sourceIds.Contains(n.sourceId));
                    IEnumerable<IdentityTest2.Models.News1> newsList = newsModel.ToList().OrderByDescending(s => s.crawlTime);
                    return View(newsList.ToPagedList(page ?? 1, 5));
                }     
            }
        }

        // GET: News1/Like/5
        [Authorize]
        public ActionResult Like(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News1 news = db.News1.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = User.Identity.GetUserId<int>();
            ViewBag.newsID = id;
            return PartialView(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult LikeNews(int id)
        {
            var userID = User.Identity.GetUserId<int>();

            if (userID != 0)
            {
                News1 news = db.News1.Find(id);
                news.numOfLikes++;
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "News1", new { id = id });
            }
            else {
                return RedirectToAction("Login", "Account");
            }
        }
        // GET: News1/HotNews/5
        [Authorize]
        public ActionResult HotNews(string sortOrder, int? page)
        {

            int userId = User.Identity.GetUserId<int>();
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            var selectedCategoriesModel = db.Categories.Include("News1").Where(c => c.AspNetUser_Category.Any(u => u.userId == userId));
            var selectedSourcesModel = db.Sources.Include("News1").Where(s => s.User_Source.Any(u => u.userId == userId));
            IEnumerable<Category> selectedCategories = selectedCategoriesModel.ToList();
            IEnumerable<Source> selectedSources = selectedSourcesModel.ToList();
            int nbcat = selectedCategories.Count();
            int nbsources = selectedSources.Count();
            ViewBag.nbcat = nbcat;
            ViewBag.nbsources = nbsources;
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
            return View(news.ToList().ToPagedList(page ?? 1, 5));
        }

        [AllowAnonymous]
        public ActionResult SearchForNews(string searchString, int? page)
        {

            var searchedNews = from s in db.News1
                               select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchedNews = searchedNews.Where(n => n.newsTitle.Contains(searchString));
            }

            //return View(searchedNews);
            return PartialView(searchedNews.ToList().ToPagedList(page ?? 1, 5));
        }

        [Authorize]
        [HttpPost, ActionName("AddSubscription")]
        public ActionResult AddSubscription(int id, [Bind(Include = "usersourceId,UserId,souceId,subscribeTime")] AspNetUser_Source aspNetUser_Source)
        {
            int userId = User.Identity.GetUserId<int>();
            bool isSubscribe = false;
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            else {
                News1 news = db.News1.Find(id);
                IEnumerable<AspNetUser_Source> aspNetUser_Sources = db.AspNetUser_Source.Where(n => n.UserId == userId);
                foreach (var row in aspNetUser_Sources)
                {
                    if (news.Source.sourceId == row.souceId)
                    {
                        isSubscribe = true;                       
                    }
                }
                if (ModelState.IsValid && !isSubscribe)
                {

                    aspNetUser_Source.UserId = userId;
                    aspNetUser_Source.subscribeTime = DateTime.Now;
                    aspNetUser_Source.souceId = news.Source.sourceId;
                    db.AspNetUser_Source.Add(aspNetUser_Source);
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("Details", "News1", new { id = id });
            }
        }
 

        [Authorize]
        [HttpPost, ActionName("UnSubscription")]
        public ActionResult UnSubscription(int id)
        {
            News1 news = db.News1.Find(id);
            int userId = User.Identity.GetUserId<int>();
            if (userId == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            else {               
                IEnumerable<AspNetUser_Source> aspNetUser_Sources = db.AspNetUser_Source.Where(n => n.UserId == userId).Where(n=>n.souceId==news.sourceId);

                if (ModelState.IsValid )
                {
                    foreach (var row in aspNetUser_Sources)
                    {
                        db.AspNetUser_Source.Remove(row);
                        
                    }
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("Details", "News1", new { id = id });
            }
        }

    }
}