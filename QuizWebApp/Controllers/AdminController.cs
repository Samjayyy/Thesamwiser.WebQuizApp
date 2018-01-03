using System.Linq;
using System.Web.Mvc;
using QuizWebApp.Code;
using QuizWebApp.Models;
using System;

namespace QuizWebApp.Controllers
{
    [AuthorizeQuizMaster]
    public class AdminController : Controller
    {
        private QuizWebAppDb DB { get; set; }

        public AdminController()
        {
            DB = new QuizWebAppDb();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new AdminFlowViewModel(this.DB));
        }

        [HttpGet]
        public ActionResult QuestionBody()
        {
            var context = DB.Context;
            return PartialView(context.CurrentQuestion);
        }

        [HttpPost]
        public ActionResult CurrentQuestion(int questionId)
        {
            var context = this.DB.Context;
            context.CurrentQuestion = DB.Questions.Find(questionId);
            this.DB.SaveChanges();

            return Json(new { });
        }
        [HttpPost]
        public ActionResult CurrentState(ContextStateType state)
        {
            var context = this.DB.Context;
            if (state != ContextStateType.PleaseWait
                && context.CurrentQuestion == null)
            {
                throw new InvalidOperationException("Can't change the state when there is no current question selected");
            }
            context.CurrentState = state;

            // if change state to "3:show answer", judge to all players.
            if (state == ContextStateType.ShowCorrectAnswer)
            {
                var answers = DB
                    .Answers
                    .Where(a => a.Question.QuestionId == context.CurrentQuestion.QuestionId)
                    .ToList();
                answers
                    .ForEach(a => a.Status =
                        a.ChosenOptionIndex == context.CurrentQuestion.IndexOfCorrectOption
                        ? AnswerStateType.Correct : AnswerStateType.Incorrect);
            }
            DB.SaveChanges();

            return Json(new { });
        }

        protected override void Dispose(bool disposing)
        {
            this.DB.Dispose();
            base.Dispose(disposing);
        }
    }
}
