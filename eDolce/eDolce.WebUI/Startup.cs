using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eDolce.WebUI.Startup))]
namespace eDolce.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
