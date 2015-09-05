using System;

namespace QuizWebApp.Models
{
    public class User
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public DateTime? AttendAsPlayerAt { get; set; }

        public bool IsAdmin { get; set; }

        public string Pass { get; set; }
    }
}