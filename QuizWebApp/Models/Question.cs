using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace QuizWebApp.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public string OwnerUserId { get; set; }

        [Display(Name = "Problem text"), Required, AllowHtml]
        public string Body { get; set; }

        [Display(Name = "Answer option 1"), Required, AllowHtml]
        public string Option1 { get; set; }
        [Display(Name = "Image option 1")]
        public string OptionImage1 { get; set; }

        [Display(Name = "Answer option 2"), Required, AllowHtml]
        public string Option2 { get; set; }
        [Display(Name = "Image option 2")]
        public string OptionImage2 { get; set; }

        [Display(Name = "Answer option 3"), AllowHtml]
        public string Option3 { get; set; }
        [Display(Name = "Image option 3")]
        public string OptionImage3 { get; set; }
        
        [Display(Name = "Answer option 4"), AllowHtml]
        public string Option4 { get; set; }
        [Display(Name = "Image option 4")]
        public string OptionImage4 { get; set; }

        public OptionViewModel[] GetOptions(bool trim = true)
        {
            Func<OptionViewModel, bool> filter = trim ?
                (opt => string.IsNullOrEmpty(opt.Option) == false) :
                (Func<OptionViewModel, bool>)(_ => true);

            return new[] { 
                new OptionViewModel(1, Option1, OptionImage1),
                new OptionViewModel(2, Option2, OptionImage2),
                new OptionViewModel(3, Option3, OptionImage3),
                new OptionViewModel(4, Option4, OptionImage4)
            }.Where(filter).ToArray();
        }

        [Display(Name = "Correct option number")]
        public int IndexOfCorrectOption { get; set; }

        [Display(Name = "Comment"), AllowHtml]
        public string Comment { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreateAt { get; set; }

        public Question()
        {
            this.CreateAt = DateTime.UtcNow;
        }
    }
}