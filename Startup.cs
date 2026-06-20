using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Crud_agenda.Startup))]
namespace Crud_agenda
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
