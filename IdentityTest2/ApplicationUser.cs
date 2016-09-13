using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityTest2
{
    public class ApplicationUser : IdentityUser
    {
        public virtual string Email { get; set; } // get user email
        public virtual string UserName { get; set; }
    }
}