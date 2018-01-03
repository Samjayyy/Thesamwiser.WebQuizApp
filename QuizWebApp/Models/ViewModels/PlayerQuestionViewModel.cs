using System.Collections.Generic;

namespace QuizWebApp.Models
{
    public class PlayerQuestionViewModel
    {
        public Question Question { get; set; }

        public Answer Answer { get; set; }

        public IEnumerable<int> PossibleValues { get; set; }

        public ContextStateType CurrentState { get; set; }
    }
}