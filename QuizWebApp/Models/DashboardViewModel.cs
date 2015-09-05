using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizWebApp.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<PlayerViewModel> Players { get; set; }

        public ILookup<string, Answer> Answers { get; set; }

        public IEnumerable<Question> Questions { get; set; }

        public DashboardViewModel(QuizWebAppDb db)
        {
            Answers = db.Answers.ToLookup(a => a.PlayerID);
            Questions = db.Questions.ToArray();

            var users = db.Users.ToArray();
            Players = users
                .Where(user =>
                    Answers[user.UserId].Any() ||
                    DateTime.UtcNow.AddMinutes(-30) <= user.AttendAsPlayerAt
                ).Select(user => new PlayerViewModel
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    CurrentScore = Answers[user.UserId].Where(a => a.Status == AnswerStateType.Correct).Sum(a => a.AssignedValue)
                })
                .OrderByDescending(player => player.CurrentScore)
                .ToArray();
        }
    }
}