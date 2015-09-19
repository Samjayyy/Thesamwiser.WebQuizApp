using System.Collections.Generic;
using System.Linq;

namespace QuizWebApp.Models
{
    public class CurrentDashboardViewModel
    {
        public CurrentDashboardViewModel(QuizWebAppDb db)
        {
            var context = db.Contexts.First();
            Question = db.Questions.Find(context.CurrentQuestionId);
            var playerIds = new HashSet<string>(db.Answers.Select(a => a.PlayerId).Distinct());
            Answers = db.Answers.Where(a => a.QuestionId == Question.QuestionId).ToDictionary(a => a.PlayerId);

            var users = db.Users.ToArray();
            Players = users
                .Where(user => !user.IsAdmin && playerIds.Contains(user.UserId))
                .ToArray();
        }
        // all players who have at least one answer somewhere, admins excluded
        public User[] Players { get; set; }

        // dictionary of all answers to the current question, with the userid as the key
        public IDictionary<string, Answer> Answers { get; set; }

        // current question
        public Question Question { get; set; }  
    }
}