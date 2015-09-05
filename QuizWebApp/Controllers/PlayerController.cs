using System;
using System.Linq;
using System.Web.Mvc;
using QuizWebApp.Models;

namespace QuizWebApp.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        public QuizWebAppDb DB { get; set; }

        public PlayerController()
        {
            DB = new QuizWebAppDb();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var userInfo = DB.Users.Find(User.Identity.UserId());
            userInfo.AttendAsPlayerAt = DateTime.UtcNow;
            DB.SaveChanges();

            return View(DB);
        }

        [HttpGet]
        public ActionResult PlayerMainContent()
        {
            var context = DB.Contexts.First();
            var playerID = User.Identity.UserId();
            var questionID = context.CurrentQuestionID;
            var answer = DB.Answers.FirstOrDefault(a => a.PlayerID == playerID && a.QuestionID == questionID);
            if (answer == null)
            {
                answer = new Answer { PlayerID = playerID, QuestionID = questionID, ChoosedOptionIndex = -1 };
                DB.Answers.Add(answer);
                DB.SaveChanges();
            }

            return PartialView("PlayerMainContent_" + context.CurrentState.ToString(), DB);
        }
    }
}
