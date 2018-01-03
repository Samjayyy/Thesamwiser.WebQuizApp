using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizWebApp.Models
{
    public class Answer
    {
        [Key, Required]
        public int AnswerId { get; set; }

        [StringLength(32)]
        [Index("IX_Answer", 1, IsUnique = true), Required] // INDEX ZIT NU CORRECT IN MIGRATIONS
        public string PlayerId { get; set; }

        [Index("IX_Answer", 2, IsUnique = true), Required] // INDEX ZIT NU CORRECT IN MIGRATIONS
        public Question Question { get; set; }

        [Required]
        public int ChosenOptionIndex { get; set; }

        [Required]
        public int AssignedValue { get; set; }

        [Required]
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