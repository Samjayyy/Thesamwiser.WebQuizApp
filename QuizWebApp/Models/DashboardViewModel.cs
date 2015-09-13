using System.Collections.Generic;
using System.Linq;

namespace QuizWebApp.Models
{
    public class DashboardViewModel
    {
        public PlayerViewModel[] Players { get; set; }

        public IDictionary<string, Dictionary<int, Answer>> Answers { get; set; }

        public IEnumerable<Question> Questions { get; set; }

        public DashboardViewModel(QuizWebAppDb db)
        {
            Questions = db.Questions.ToArray();
            var answersPerPlayer = db.Answers.ToLookup(all => all.PlayerID);
            Answers = answersPerPlayer.ToDictionary(perplayer => perplayer.Key, perplayer => perplayer.ToDictionary(q => q.QuestionID));

            var users = db.Users.ToArray();
            Players = users
                .Where(user => !user.IsAdmin && Answers.ContainsKey(user.UserId))
                .Select(user => new PlayerViewModel
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    CurrentScore = answersPerPlayer[user.UserId]
                        .Where(a => a.Status == AnswerStateType.Correct)
                        .Sum(a => a.AssignedValue)
                })
                .OrderByDescending(player => player.CurrentScore)
                .ToArray();

            CalculateColors(Players);
        }

        private static void CalculateColors(PlayerViewModel[] players)
        {
            // calculate fancy colors
            for (var p = 0; p < players.Length; p++)
            {
                if (p > 0 && players[p].CurrentScore == players[p - 1].CurrentScore)
                {
                    players[p].Green = players[p - 1].Green;
                    players[p].Red = players[p - 1].Red;
                }
                else
                {
                    var val = (255 * p) / (players.Length - 1);
                    players[p].Green = 255 - val;
                    players[p].Red = val;
                }
            }
        }
    }
}