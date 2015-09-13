namespace QuizWebApp.Models
{
    public class PlayerViewModel
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public int CurrentScore { get; set; }

        public int Green { get; set; } = 255;

        public int Red { get; set; } = 0;
    }
}