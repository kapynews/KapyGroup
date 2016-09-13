using IdentityTest2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(IdentityTest2.Startup))]
namespace IdentityTest2 
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var myUserStore = new CustomUserStore(context);
            var myRoleStore = new CustomRoleStore(context);
            var roleManager = new ApplicationRoleManager(myRoleStore);
            var UserManager = new ApplicationUserManager(myUserStore);

            var default_adminName = "KapyAdmin";
            var default_adminEmail = "kapynews@gmail.com";
            var default_adminRole = "admin";
            var default_memberRole = "member";


            string default_adminPWD = "Kapyiscool1234";
            try
            {
                var AdminUser = context.Users.SingleOrDefault(u => u.UserName == default_adminName);
                var AdminRole = context.Roles.SingleOrDefault(r => r.Name == default_adminRole);
                // In Startup iam creating first Admin Role and creating a default Admin User    
                if (AdminRole == null)
                {
                    roleManager.CreateAsync(new CustomRole(default_adminRole)).Wait();
                    AdminRole = context.Roles.SingleOrDefault(r => r.Name == default_adminRole);

                    //Here we create an Admin super user who will maintain the website 
                    if (AdminUser == null)
                    {
                        UserManager.CreateAsync(new ApplicationUser { UserName = default_adminName, Email = default_adminEmail, EmailConfirmed = true }, default_adminPWD).Wait();
                        AdminUser = context.Users.SingleOrDefault(u => u.UserName == default_adminName);
                    }
                    var userRole = AdminUser.Roles.SingleOrDefault(r => r.RoleId == AdminRole.Id);
                    if (userRole == null)
                    {
                        UserManager.AddToRoleAsync(AdminUser.Id, AdminRole.Name).Wait();
                    }
                }

                var MemberRole = context.Roles.SingleOrDefault(r => r.Name == default_memberRole);
                if (MemberRole == null)
                {
                    roleManager.CreateAsync(new CustomRole(default_memberRole)).Wait();
                    MemberRole = context.Roles.SingleOrDefault(r => r.Name == default_memberRole);
                }


                }


            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
