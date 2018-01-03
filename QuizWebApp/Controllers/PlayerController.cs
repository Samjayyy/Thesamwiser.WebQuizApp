using System.Linq;
using System.Web.Mvc;
using QuizWebApp.Models;
using System;
using System.Data.Entity;

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
            return View(DB.Context);
        }

        /// <summary>
        /// Get all data to show to the players
        /// but this may fail when there is no question created yet
        /// </summary>
        [HttpGet]
        public ActionResult PlayerMainContent()
        {
            var context = this.DB.Context;
            var playerId = User.Identity.UserId();
            var question = context.CurrentQuestion;
            Answer answer = null;
            if (question != null)
            {
                answer = DB.Answers.FirstOrDefault(a => a.PlayerId == playerId && a.Question.QuestionId == question.QuestionId)
                                ?? new Answer { PlayerId = playerId, Question = question, ChosenOptionIndex = -1, Status = AnswerStateType.NoEntry };
            }
            var model = new PlayerQuestionViewModel
            {
                Question = question,
                Answer = answer,
                PossibleValues = context.ValueAssigner.GetPossibleValues(DB, playerId, question)

            };
            return PartialView("PlayerMainContent_" + context.CurrentState.ToString(), model);
        }

        [HttpPost]
        public ActionResult PlayerSelectedOptionIndex(int answerId, int answerIndex, int assignedValue)
        {
            try
            {
                SelectAnswer(answerId, answerIndex, assignedValue, User.Identity.UserId());
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
        private bool SelectAnswer(int answerId, int answerIndex, int assignedValue, string playerId)
        {
            var context = this.DB.Context;
            if (context.CurrentState != ContextStateType.ChooseTheAnswer)
            {
                return false; // Illegal state: can not answer question in other state than ChooseTheAnswer
            }
            var question = context.CurrentQuestion;
            if (question == null)
            {
                return false; // can't add an answer to a non existing question
            }
            Answer answer = null;
            if (answerId > 0)
            {
                // 1. By Id
                answer = DB.Answers
                    .Find(answerId);
                if (answer != null
                    && (answer.PlayerId != playerId
                        || answer.Question.QuestionId != question.QuestionId))
                {
                    return false; // you shouldn't answer in someone else's name, or to another question at this time
                }
            }
            if (answer == null)
            {
                // 2. By Unique business key
                answer = DB.Answers
                    .Include(a => a.Question)
                    .FirstOrDefault(a => a.PlayerId == playerId && a.Question.QuestionId == question.QuestionId);
            }
            if (answer == null)
            {
                // 3. adding new
                answer = new Answer { PlayerId = playerId, Question = question, ChosenOptionIndex = -1, Status = AnswerStateType.NoEntry, AssignedValue = 0 };
                DB.Answers.Add(answer);
            }
            // check if the assignedValue is allowed, and take anotherone instead
            answer.AssignedValue = context.ValueAssigner.ValidateAssignedValue(DB, assignedValue, playerId, question);
            answer.ChosenOptionIndex = answerIndex;
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
