using System.Collections.Generic;
using System.Linq;

namespace QuizWebApp.Models
{
    public class DashboardViewModel
    {
        public PlayerViewModel[] Players { get; set; }

        public IDictionary<string, Dictionary<int, Answer>> Answers { get; set; }

        public IList<QuestionRoundViewModel> QuestionRounds { get; set; }

        public DashboardViewModel(QuizWebAppDb db)
        {
            // get questions per round
            var rounds = db.Rounds.ToList();
            var questions = db.Questions.ToLookup(q => q.RoundId);
            QuestionRounds = rounds
                .OrderBy(r => r.SortOrder)
                .Select(r => new QuestionRoundViewModel { Round = r, Questions = questions[r.RoundId].OrderBy(q => q.SortOrder) })
                .ToList();
            // get all answers to questions
            var answersPerPlayer = db.Answers.ToLookup(all => all.PlayerId);
            Answers = answersPerPlayer.ToDictionary(perplayer => perplayer.Key, perplayer => perplayer.ToDictionary(q => q.Question.QuestionId));
            // get all users
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
                    var val = (255 * p) / players.Length;
                    players[p].Green = 255 - val;
                    players[p].Red = val;
                }
            }
        }
    }
}