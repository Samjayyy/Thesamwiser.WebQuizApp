using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(QuizWebApp.Startup))]

namespace QuizWebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
