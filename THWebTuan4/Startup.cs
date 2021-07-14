using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(THWebTuan4.Startup))]
namespace THWebTuan4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
