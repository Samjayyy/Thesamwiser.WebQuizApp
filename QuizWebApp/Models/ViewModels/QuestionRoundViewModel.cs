using System.Collections.Generic;

namespace QuizWebApp.Models
{
    public class QuestionRoundViewModel
    {
        public Round Round { get; set; }
        public IEnumerable<Question> Questions { get; set; }

    }
}