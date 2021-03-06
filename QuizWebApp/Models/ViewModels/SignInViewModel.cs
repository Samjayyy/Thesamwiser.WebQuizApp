﻿using System.ComponentModel.DataAnnotations;

namespace QuizWebApp.Models
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {
        }

        [Display(Name = "Handle name"), Required]
        public string HandleName { get; set; }

        [Display(Name = "Pass"), Required]
        public string Pass { get; set; }
    }
}