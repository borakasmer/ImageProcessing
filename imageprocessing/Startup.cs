using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ImageProcessing.Startup))]
namespace ImageProcessing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
