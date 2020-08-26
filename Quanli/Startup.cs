using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Quanli.Startup))]
namespace Quanli
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
