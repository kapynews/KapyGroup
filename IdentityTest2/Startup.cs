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
        }
    }
}
