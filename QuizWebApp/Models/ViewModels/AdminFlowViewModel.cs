using System.Linq;
using System.Web.Mvc;

namespace QuizWebApp.Models
{
    public class AdminFlowViewModel
    {
        private readonly SelectListItem[] _states, _questions;
        public AdminFlowViewModel(QuizWebAppDb db)
        {
            var context = db.Context;
            // make sure there is a current question and it is set correctly
            if (context.CurrentQuestion == null
                && db.Questions.Any())
            {
                // Not completely correct, shouldn't be on roundid but on sortorder of the linked round
                context.CurrentQuestion = db.Questions
                    .OrderBy(q => q.RoundId).ThenBy(q => q.SortOrder)
                    .FirstOrDefault();
                db.SaveChanges();
            }
            var curQuestion = context.CurrentQuestion;
            // create new list of selectlistitems for the state of the question
            _states = new[] {
                new SelectListItem{ Value="PleaseWait", Text="Please wait"},
                new SelectListItem{ Value="ChooseTheAnswer", Text="Choose the answer"},
                new SelectListItem{ Value="Closed", Text="Closed"},
                new SelectListItem{ Value="ShowCorrectAnswer", Text="Show correct answer"},
            };
            // set current selected state in drop down
            var curState = _states.FirstOrDefault(s => s.Value == context.CurrentState.ToString());
            if(curState != null)
            {
                curState.Selected = true;
            }
            // set available questions and the currently selected one
            _questions = db.Questions
                .ToArray()
                .Select((q, i) => new SelectListItem
                {
                    Value = q.QuestionId.ToString(),
                    Text = "Q" + (i + 1).ToString(),
                    Selected = curQuestion != null ? curQuestion.QuestionId == q.QuestionId : false
                })
                .ToArray();
        }
        public SelectListItem[] States => _states;
        public SelectListItem[] Questions => _questions;
    }
}