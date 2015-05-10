using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BasisForAppraisal_finalProject.Startup))]
namespace BasisForAppraisal_finalProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
