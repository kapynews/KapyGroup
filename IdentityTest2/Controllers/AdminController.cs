using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityTest2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace IdentityTest2.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext context;


        public AdminController()
        {
            context = new ApplicationDbContext();
        }

        // GET: RolesAndUsers
        [AllowAnonymous]
        public ActionResult Index(string sortOrder, int? page)
        {

            ViewBag.DateSortParm = sortOrder == "ID" ? "Time" : "ID";
            var User_model = from u in context.Users select u;


            ViewBag.message = "List of all users";


            return View(User_model.ToList().ToPagedList(page ?? 1, 5));

        }
    }
}