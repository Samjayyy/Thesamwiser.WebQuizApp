using QuizWebApp.Models;
using System.Collections.Generic;

namespace QuizWebApp.Logic.ValuesAssigner
{
    public interface IValueAssigner
    {
        int ValidateAssignedValue(QuizWebAppDb DB, int assignedValue, string playerId, Question currentQuestion);
        IEnumerable<int> GetPossibleValues(QuizWebAppDb DB, string playerId, Question currentQuestion);
    }
}
