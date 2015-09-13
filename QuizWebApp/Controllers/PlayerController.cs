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
                answer = new Answer { PlayerID = playerID, QuestionID = questionID, ChosenOptionIndex = -1 };
                DB.Answers.Add(answer);
                DB.SaveChanges();
            }
            var model = new PlayerQuestionViewModel
            {
                Answer = answer,
                Question = DB.Questions.Find(questionID),
            };
            return PartialView("PlayerMainContent_" + context.CurrentState.ToString(), model);
        }

        [HttpPost]
        public ActionResult PlayerSelectedOptionIndex(int answerIndex)
        {
            var context = DB.Contexts.First();
            if (context.CurrentState == ContextStateType.ChooseTheAnswer)
            { 
                // still in legal state
                var playerId = User.Identity.UserId();
                var questionId = context.CurrentQuestionID;
                var answer = DB.Answers.First(a => a.PlayerID == playerId && a.QuestionID == questionId);
                answer.ChosenOptionIndex = answerIndex;
                answer.AssignedValue = 1;
                answer.Status = AnswerStateType.Pending;/*entried*/
                DB.SaveChanges();
            }
            return Json(new { });
        }

        protected override void Dispose(bool disposing)
        {
            this.DB.Dispose();
            base.Dispose(disposing);
        }
    }
}
