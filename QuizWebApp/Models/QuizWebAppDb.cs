using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace QuizWebApp.Models
{
    public class QuizWebAppDb : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Context> Contexts { get; set; }

        public IEnumerable<int> GetPublicQuestionIds()
        {
            var publicQuestionIds = this.Answers
                .Where(a => new[] { AnswerStateType.Correct, AnswerStateType.Incorrect }.Contains(a.Status))
                .Select(a => a.QuestionID)
                .Distinct()
                .ToArray();
            return publicQuestionIds;
        }

    }
}