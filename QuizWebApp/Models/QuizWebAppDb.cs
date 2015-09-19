using System.Data.Entity;

namespace QuizWebApp.Models
{
    public class QuizWebAppDb : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Context> Contexts { get; set; }

    }
}