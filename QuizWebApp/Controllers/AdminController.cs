﻿using System.Linq;
using System.Web.Mvc;
using QuizWebApp.Code;
using QuizWebApp.Models;

namespace QuizWebApp.Controllers
{
    [AuthorizeQuizMaster]
    public class AdminController : Controller
    {
        public QuizWebAppDb DB { get; set; }

        public AdminController()
        {
            DB = new QuizWebAppDb();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(this.DB);
        }

        [HttpGet]
        public ActionResult QuestionBody()
        {
            var context = this.DB.Contexts.First();
            var curQuestion = this.DB.Questions.Find(context.CurrentQuestionID);
            return PartialView(curQuestion);
        }

        [HttpPost]
        public ActionResult CurrentQuestion(int questionID)
        {
            this.DB.Contexts.First().CurrentQuestionID = questionID;
            this.DB.SaveChanges();

            return Json(new { });
        }
        [HttpPost]
        public ActionResult CurrentState(ContextStateType state)
        {
            var context = DB.Contexts.First();
            context.CurrentState = state;

            // if change state to "3:show answer", judge to all players.
            if (state == ContextStateType.ShowCorrectAnswer)
            {
                var answers = DB
                    .Answers
                    .Where(a => a.QuestionID == context.CurrentQuestionID)
                    .ToList();
                var currentQuestion = DB.Questions.Find(context.CurrentQuestionID);

                answers
                    .ForEach(a => a.Status =
                        a.ChosenOptionIndex == currentQuestion.IndexOfCorrectOption
                        ? AnswerStateType.Correct : AnswerStateType.Incorrect);
            }
            DB.SaveChanges();

            return Json(new { });
        }


    }
}
