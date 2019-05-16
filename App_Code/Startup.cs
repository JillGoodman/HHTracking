using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HCPSS.Startup))]
namespace HCPSS
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
