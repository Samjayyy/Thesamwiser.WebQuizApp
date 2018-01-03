using QuizWebApp.Logic.ValuesAssigner;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizWebApp.Models
{
    /// <summary>
    /// The context of the quiz aka Quiz Flow
    /// -> For now only one row may exist, containing the information about 
    ///     - which question is currently showing, in 
    ///     - in which state
    ///     - if the dashboard is available for normal users
    /// </summary>
    public class Context
    {
        public Context()
        {
            ValueAssigner = new UniqueScoreChosenByPlayer();
        }

        [Key]
        public int ContextId { get; set; }

        public Question CurrentQuestion { get; set; }

        public ContextStateType CurrentState { get; set; }

        public bool IsDashboardAvailableForUsers { get; set; }

        public bool ShowAssignedValueInDashboard { get; set; }

        [NotMapped]
        public IValueAssigner ValueAssigner { get; set; }

        public string ShowAssignedValue(int value)
        {
            if (!ShowAssignedValueInDashboard)
            {
                return " ";
            }
            return value+"";
        }
    }

    /// <summary>
    /// The possible states of the quiz
    /// In order:
    ///     -> Waiting for the next question
    ///     -> Choosing the correct answer
    ///     -> Answering closed, waiting to reveal correct answers
    ///     -> Show the correct answer, and optional some extra comments
    /// </summary>
    public enum ContextStateType
    {
        PleaseWait,
        ChooseTheAnswer,
        Closed,
        ShowCorrectAnswer
    }
}