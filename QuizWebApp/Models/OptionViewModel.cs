﻿namespace QuizWebApp.Models
{
    public class OptionViewModel
    {
        public int OptionNumber { get; set; }

        public string Option { get; set; }

        public string OptionImage { get; set; }

        public OptionViewModel(int optionNumber, string option, string optionImage)
        {
            this.OptionNumber = optionNumber;
            this.Option = option;
            this.OptionImage = optionImage;
        }
    }
}