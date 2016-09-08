using IdentityTest2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;

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


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("admin"))
            {

                // first we create Admin rool   
                var role = new CustomRole();
                role.Id = 1;
                role.Name = "admin";
                roleManager.Create(role);

                //Here we create an Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "KapyAdmin";
                user.Email = "kapynews@gmail.com";

                string userPWD = "Kapyiscool1234";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "admin");

                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("member"))
            {

                var role = new CustomRole();
                role.Id = 2;
                role.Name = "member";
                roleManager.Create(role);

            }

            
        }
    }
}
