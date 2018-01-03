using System.Collections.Generic;
using System.Linq;

namespace QuizWebApp.Models
{
    public class CurrentDashboardViewModel
    {
        public CurrentDashboardViewModel(QuizWebAppDb db)
        {
            Context = db.Context;
            var question = Context.CurrentQuestion;
            var playerIds = new HashSet<string>(db.Answers.Select(a => a.PlayerId).Distinct());
            Answers = db.Answers.Where(a => a.Question.QuestionId == question.QuestionId).ToDictionary(a => a.PlayerId);

            var users = db.Users.ToArray();
            Players = users
                .Where(user => !user.IsAdmin && playerIds.Contains(user.UserId))
                .ToArray();
        }
        // all players who have at least one answer somewhere, admins excluded
        public User[] Players { get; set; }

        // dictionary of all answers to the current question, with the userid as the key
        public IDictionary<string, Answer> Answers { get; set; }

        public Context Context { get; set; }  
    }
}