using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QRCode.Startup))]
namespace QRCode
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
