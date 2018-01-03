using System.Data.Entity;
using System.Linq;

namespace QuizWebApp.Models
{
    public class QuizWebAppDb : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Round> Rounds { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Context> Contexts { get; set; }

        public Context Context {
            get
            {
                return Contexts
                    .Include(c=>c.CurrentQuestion)
                    .First();
            }
        }
        internal void InitContext()
        {
            if (!Contexts.Any())
            {
                // set values before running or adjust in database manually afterwards
                Contexts.Add(new Context { CurrentQuestion = null, CurrentState = ContextStateType.PleaseWait, IsDashboardAvailableForUsers = true, ShowAssignedValueInDashboard = true });
                SaveChanges();
            }
        }
    }
}