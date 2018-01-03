using System;
using System.Linq;
using System.Web.Mvc;
using QuizWebApp.Models;

namespace QuizWebApp.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private QuizWebAppDb DB { get; set; }

        public QuestionController()
        {
            this.DB = new QuizWebAppDb();
        }

        [HttpGet]
        public ActionResult Index(int roundId)
        {
            var round = DB.Rounds.Find(roundId);
            var questions = DB.Questions
                .Where(q => q.RoundId == roundId)
                .OrderBy(q => q.SortOrder)
                .ToArray();
            return View(new QuestionRoundViewModel
            {
                Round = round,
                Questions = questions
            });
        }

        [HttpGet]
        public ActionResult Overview(int roundId)
        {
            var round = DB.Rounds.Find(roundId);
            var questions = DB.Questions
                .Where(q => q.RoundId == roundId)
                .OrderBy(q => q.SortOrder)
                .ToArray();
            ViewBag.ShowAll = Request.QueryString["ShowAll"] != null;
            return View(new QuestionRoundViewModel
            {
                Round = round,
                Questions = questions
            });
        }


        [HttpGet]
        public ActionResult Create(int roundId)
        {
            return View(new Question { RoundId = roundId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            model.OwnerUserId = User.Identity.UserId();
            model.CreateAt = DateTime.UtcNow;
            model.SortOrder = DB.Questions.Count(q => q.RoundId == model.RoundId)+1;
            DB.Questions.Add(model);
            DB.SaveChanges();
            return RedirectToAction("Index",new { roundId = model.RoundId});
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var question = DB.Questions.Find(id);
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Question model)
        {
            var question = this.DB.Questions.Find(id);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UpdateModel(question, 
                prefix: null, 
                includeProperties: null,
                excludeProperties: new[] { "QuestionId", "OwnerUserId", "CreateAt" });

            DB.SaveChanges();
            return RedirectToAction("Index", new { roundId = model.RoundId });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var question = DB.Questions.Find(id);
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection _)
        {
            var question = this.DB.Questions.Find(id);
            if(question == null)
            {
                throw new InvalidOperationException($"Trying to delete question with id {id}, but does not exist anymore.");
            }
            this.DB.Questions.Remove(question);
            this.DB.SaveChanges();
            return RedirectToAction("Index", new { roundId = question.RoundId });
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var question = DB.Questions.Find(id);
            return View(question);
        }

        protected override void Dispose(bool disposing)
        {
            this.DB.Dispose();
            base.Dispose(disposing);
        }

    }
}