using QuizWebApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuizWebApp.Logic.ValuesAssigner
{
    public class AllEquallyScored : IValueAssigner
    {
        public int ValidateAssignedValue(QuizWebAppDb DB, int assignedValue, string playerId, Question currentQuestion)
        {
            return 1;
        }
        public IEnumerable<int> GetPossibleValues(QuizWebAppDb DB, string playerId, Question currentQuestion)
        {
            return Enumerable.Range(1, 1);
        }
    }
}
