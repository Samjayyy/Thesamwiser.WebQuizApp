namespace QuizWebApp.Models
{
    public class Answer
    {
        public int AnswerID { get; set; }

        public string PlayerID { get; set; }

        public int QuestionID { get; set; }

        public int ChoosedOptionIndex { get; set; }

        public int AssignedValue { get; set; }

        /// <summary>
        /// 0: no entry.
        /// 1: pending.
        /// 2: correct.
        /// 3: incorrect.
        /// </summary>
        public AnswerStateType Status { get; set; }
    }
}