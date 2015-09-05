using System.Web.Mvc;
using QuizWebApp.Code;

namespace QuizWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.IsQuizMaster = AuthorizeQuizMaster.IsAllow(this.HttpContext);
            return View();
        }
    }
}
