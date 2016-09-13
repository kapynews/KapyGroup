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


        

        
        public async System.Threading.Tasks.Task<ActionResult> Index(string sortOrder, int? page)

        {
            var myRoleStore = new CustomRoleStore(context);
            List<UserRoles> usersWithRoles = new List<UserRoles>();

            ViewBag.DateSortParm = sortOrder == "ID" ? "Time" : "ID";
            var User_model = from u in context.Users select u;
            var userList = User_model.ToList();

            foreach (var user in userList)
            {
                var userRoles = user.Roles;
                List<String> roleList = new List<string>();
                if (userRoles.Count == 0) {
                    roleList.Add("No role assigned");
                }else
                {
                    foreach (var r in userRoles)
                    {
                        CustomRole _role = await myRoleStore.FindByIdAsync(r.RoleId);
                        var rolename = _role.Name;
                        roleList.Add(rolename);
                    }
                    
                }

                usersWithRoles.Add(new UserRoles(user.UserName, user.Email, user.UserPhoto, roleList));

            }
            ViewBag.message = "Manage Users and Roles";



            //return View(User_model.ToList().ToPagedList(page ?? 1, 20));
            return View(usersWithRoles.ToPagedList(page ?? 1, 10));

        }
    }
}