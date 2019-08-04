using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PFSList.Startup))]
namespace PFSList
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
