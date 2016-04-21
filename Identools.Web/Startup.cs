using AutoMapper;
using Identools.Web.Automapper;
using Owin;

namespace Identools.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            ConfigureAutoMapper();
        }

        private void ConfigureAutoMapper()
        {
            Mapper.Initialize(mc => mc.AddProfile(new UndeMancamProfile()));
        }
    }
}