using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizWebApp.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }

        [StringLength(32)]
        [Index("IX_Answer", 1, IsUnique = true)]
        public string PlayerId { get; set; }

        [Index("IX_Answer", 2, IsUnique = true)]
        public int QuestionId { get; set; }

        public int ChosenOptionIndex { get; set; }

        public int AssignedValue { get; set; }

        public AnswerStateType Status { get; set; }
    }

    /// <summary>
    /// Possible states for answers
    /// </summary>
    public enum AnswerStateType
    {
        NoEntry,
        Pending,
        Correct,
        Incorrect
    }
}