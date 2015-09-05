﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using QuizWebApp.Models;

namespace QuizWebApp.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        public QuizWebAppDb DB { get; set; }

        public QuestionController()
        {
            this.DB = new QuizWebAppDb();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var questions = DB.Questions
                .OrderBy(q => q.CreateAt)
                .ToArray();
            return View(questions);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Question());
        }

        [HttpPost]
        public ActionResult Create(Question model)
        {
            if (IsValidDataURL(model) == false) throw new ApplicationException("Invalid Data URL.");

            if(!ModelState.IsValid)
            {
                return View(model);
            }
            model.OwnerUserId = User.Identity.UserId();
            model.CreateAt = DateTime.UtcNow;
            DB.Questions.Add(model);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var question = DB.Questions.Find(id);
            return View(question);
        }

        [HttpPost]
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

            if (IsValidDataURL(question) == false) throw new ApplicationException("Invalid Data URL.");

            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool IsValidDataURL(Question model)
        {
            return model.GetOptions(trim: false)
                .Where(opt => !string.IsNullOrWhiteSpace(opt.OptionImage))
                .Select(opt => opt.OptionImage)
                .All(url => Regex.IsMatch(url, @"(^data:image/\w+;\w+,[0-9a-zA-Z/+=]+$)|(^$)"));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var question = DB.Questions.Find(id);
            return View(question);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection _)
        {
            var question = this.DB.Questions.Find(id);
            this.DB.Questions.Remove(question);
            this.DB.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var question = DB.Questions.Find(id);
            return View(question);
        }

    }
}