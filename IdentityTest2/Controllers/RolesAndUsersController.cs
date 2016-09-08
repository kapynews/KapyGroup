using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityTest2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityTest2.Controllers
{

    [Authorize(Roles = "admin")]
    public class RolesAndUsersController : Controller
    {
        ApplicationDbContext context;
        

        public RolesAndUsersController()
        {
            context = new ApplicationDbContext();
        }

        // GET: RolesAndUsers
        [AllowAnonymous]
        public ActionResult Index()
        {
            var UserIsAdmin = true;
            ViewBag.UserIsAdmin = true;

                return View();
           
        }
    }
}