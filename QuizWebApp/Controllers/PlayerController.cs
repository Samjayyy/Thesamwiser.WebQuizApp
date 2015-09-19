using System.Linq;
using System.Web.Mvc;
using QuizWebApp.Models;
using System;

namespace QuizWebApp.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        private QuizWebAppDb DB { get; set; }

        public PlayerController()
        {
            DB = new QuizWebAppDb();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(DB);
        }

        [HttpGet]
        public ActionResult PlayerMainContent()
        {
            var context = DB.Contexts.First();
            var playerId = User.Identity.UserId();
            var questionId = context.CurrentQuestionId;
            var answer = DB.Answers.FirstOrDefault(a => a.PlayerId == playerId && a.QuestionId == questionId)
                ?? new Answer { PlayerId = playerId, QuestionId = questionId, ChosenOptionIndex = -1, Status = AnswerStateType.NoEntry };
            var model = new PlayerQuestionViewModel
            {
                Question = DB.Questions.Find(questionId),
                Answer = answer
            };
            return PartialView("PlayerMainContent_" + context.CurrentState.ToString(), model);
        }

        [HttpPost]
        public ActionResult PlayerSelectedOptionIndex(int answerId, int answerIndex)
        {
            try
            {
                SelectAnswer(answerId, answerIndex, User.Identity.UserId());
            }
            catch (Exception ex)
            {
                // Gotta catch them all. (e.g. Concurrency exceptions on db)
                Console.WriteLine(ex.Message);
            }
            return Json(new { });
        }

        /// <summary>
        /// Throws exception in case of concurrency on database
        /// </summary>
        private bool SelectAnswer(int answerId, int answerIndex, string playerId)
        {
            var context = DB.Contexts.First();
            if (context.CurrentState != ContextStateType.ChooseTheAnswer)
            {
                return false; // Illegal state: can not answer question in other state than ChooseTheAnswer
            }
            var questionId = context.CurrentQuestionId;
            Answer answer = null;
            if (answerId > 0)
            {
                // 1. By Id
                answer = DB.Answers.Find(answerId);
                if (answer != null
                    && (answer.PlayerId != playerId
                        ||  answer.QuestionId != questionId))
                {                    
                    return false; // you shouldn't answer in someone else's name, or to another question at this time
                }
            }
            if (answer == null)
            {
                // 2. By Unique business key
                answer = DB.Answers.FirstOrDefault(a => a.PlayerId == playerId && a.QuestionId == questionId);
            }
            if (answer == null)
            {
                // 3. adding new
                answer = new Answer { PlayerId = playerId, QuestionId = questionId, ChosenOptionIndex = -1, Status = AnswerStateType.NoEntry };
                DB.Answers.Add(answer);
            }
            answer.ChosenOptionIndex = answerIndex;
            answer.AssignedValue = 1;
            answer.Status = AnswerStateType.Pending; // entried, not validated so far, important for showing on dashboard
            DB.SaveChanges();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            this.DB.Dispose();
            base.Dispose(disposing);
        }
    }
}
