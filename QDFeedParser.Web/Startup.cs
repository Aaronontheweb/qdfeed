using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QDFeedParser.Web.Startup))]
namespace QDFeedParser.Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
