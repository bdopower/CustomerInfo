using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CustomerInfoApp.Startup))]
namespace CustomerInfoApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
