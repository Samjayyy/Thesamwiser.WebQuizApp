using QuizWebApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuizWebApp.Logic.ValuesAssigner
{
    public class UniqueScoreChosenByPlayer : IValueAssigner
    {
        public int ValidateAssignedValue(QuizWebAppDb DB, int assignedValue, string playerId, Question currentQuestion)
        {
            var possibleValues = GetPossibleValues(DB, playerId, currentQuestion);
            if (possibleValues.Contains(assignedValue))
            {
                return assignedValue;
            }
            return possibleValues.FirstOrDefault();
        }
        public IEnumerable<int> GetPossibleValues(QuizWebAppDb DB, string playerId, Question currentQuestion)
        {
            if (currentQuestion == null)
            {
                return Enumerable.Range(0, 1);
            }
            var prevAnswers = DB.Answers
                .Where(a => a.PlayerId == playerId 
                    && a.Question.QuestionId != currentQuestion.QuestionId
                    && a.Question.RoundId == currentQuestion.RoundId
                    && a.Status != AnswerStateType.Pending
                    && a.Status != AnswerStateType.NoEntry)
                .Select(a => a.AssignedValue);
            var count = DB.Questions.Count(q => q.RoundId == currentQuestion.RoundId);
            return Enumerable.Range(1, count).Except(prevAnswers); // use each value only once
        }
    }
}
