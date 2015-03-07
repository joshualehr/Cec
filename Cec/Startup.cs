using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cec.Startup))]
namespace Cec
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
