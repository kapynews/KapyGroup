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
    [Authorize]
    public class AspNetUser_CategoryController : Controller
    {
        private kapymvc1Entities db = new kapymvc1Entities();

        // GET: AspNetUser_Category
        [AllowAnonymous]
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
            ViewBag.userId = User.Identity.GetUserId<int>();
           


            //ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName");
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

            //ViewBag.userId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetUser_Category.userId);
            ViewBag.userId = User.Identity.GetUserId<int>();
            //ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", aspNetUser_Category.categoryId);
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


        //[HttpPost]
        //public ActionResult ListSelectedCategories()
        //{
        //    int userId = User.Identity.GetUserId<int>();
        //    if (userId != 0)
        //    {
        //        var selectedCategoriesModel = db.Categories.Where(c => c.AspNetUser_Category.Any(u => u.userId == userId));
        //        IEnumerable<Category> selectCategories = selectedCategoriesModel.ToList();
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("You have selected categories: " );
        //        foreach (var c in selectCategories)
        //        {
        //            sb.Append(c.categoryName + " ");
        //        }
                
        //        ViewBag.message = sb.ToString();
        //        return ViewBag;
        //    }
        //    else {

        //        return ViewBag;
        //    }
        //}
        [HttpGet]
        public ActionResult Insert() {
           
            return View(db.Categories.ToList());

        }
        //private List<Category> categoryList = new kapymvc1Entities().Categories.ToList();
        [HttpPost,ActionName("Insert")]
        public ActionResult Insert(IEnumerable<Category> categories)
        {
            bool isAdded = false;
            int userId=User.Identity.GetUserId<int>();
            if (userId == 0)
            {
                ViewBag.message = "Sorry, please login or register First";
                return View("InsertResult");
            }
            else {

                if (categories == null || categories.Count(x => x.isSelected==true) == 0 )
                {
                    ViewBag.message = "You haven't selected any category";
                    return View("InsertResult");
                }
                else {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("You have successfully selected:  ");
                    foreach (Category c in categories)
                    {
                        IEnumerable<AspNetUser_Category> aspNetUser_Categories = db.AspNetUser_Category.Where(n => n.userId == userId);
                        foreach (var row in aspNetUser_Categories) {
                            if (c.categoryId == row.categoryId){
                                isAdded = true;
                            }
                        }
                        if (c.isSelected==true && !isAdded)
                        {
                            sb.Append(c.categoryName + " ");
                            if (ModelState.IsValid)
                            {
                                var save = new AspNetUser_Category
                                {
                                    userId = User.Identity.GetUserId<int>(),
                                    categoryId = c.categoryId
                                };
                                db.AspNetUser_Category.Add(save);
                                db.SaveChanges();
                            }
                            ModelState.Clear();
                        }
                        isAdded = false;
                    }
                    sb.Remove(sb.ToString().LastIndexOf(" "), 1);
                    ViewBag.message = sb.ToString();
                    //return View();
                    return View("InsertResult");
                }
            }
            

        }

    }
}
