using System.Web.Mvc;
using QuizWebApp.Models;
using QuizWebApp.Code;
using System.Linq;
using System;

namespace QuizWebApp.Controllers
{
    [AuthorizeDashboard]
    public class DashboardController : Controller
    {
        public QuizWebAppDb DB { get; set; }

        public DashboardController()
        {
            this.DB = new QuizWebAppDb();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new DashboardViewModel(this.DB);
            return View(model);
        }

        [HttpGet]
        public ActionResult LatestDashboard()
        {
            var model = new DashboardViewModel(this.DB);
            return PartialView("DashboardMainContent", model);
        }

        [HttpGet]
        public ActionResult Current()
        {
            var model = new CurrentDashboardViewModel(this.DB);
            return View(model);
        }

        [HttpGet]
        public ActionResult LatestCurrent()
        {
            var model = new CurrentDashboardViewModel(this.DB);
            return PartialView("CurrentDashboardMainContent", model);
        }

        protected override void Dispose(bool disposing)
        {
            this.DB.Dispose();
            base.Dispose(disposing);
        }
    }
}
