using System;
using System.Linq;
using System.Web.Mvc;
using QuizWebApp.Models;

namespace QuizWebApp.Controllers
{
    [Authorize]
    public class RoundController : Controller
    {
        private QuizWebAppDb DB { get; set; }

        public RoundController()
        {
            this.DB = new QuizWebAppDb();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var rounds = DB.Rounds
                .OrderBy(q => q.SortOrder)
                .ToArray();
            return View(rounds);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View(new Round());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Round model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            model.OwnerUserId = User.Identity.UserId();
            model.CreateAt = DateTime.UtcNow;
            model.SortOrder = DB.Rounds.Count()+1;
            DB.Rounds.Add(model);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var round = DB.Rounds.Find(id);
            return View(round);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Round model)
        {
            var round = this.DB.Rounds.Find(id);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UpdateModel(round, 
                prefix: null, 
                includeProperties: null,
                excludeProperties: new[] {"OwnerUserId", "CreateAt" });
            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var round = DB.Rounds.Find(id);
            return View(round);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection _)
        {
            // first remove all underlying questions
            var questions = this.DB.Questions.Where(q => q.RoundId == id);
            this.DB.Questions.RemoveRange(questions);
            // then remove round itself
            var round = this.DB.Rounds.Find(id);
            this.DB.Rounds.Remove(round);
            this.DB.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var round = DB.Rounds.Find(id);
            return View(round);
        }

        protected override void Dispose(bool disposing)
        {
            this.DB.Dispose();
            base.Dispose(disposing);
        }

    }
}