using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace QuizWebApp.Models
{
    public class Question
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public Question()
        {
            CreateAt = DateTime.UtcNow;
        }

        [Key]
        public int QuestionId { get; set; }

        [Required]
        public int RoundId { get; set; }

        public int SortOrder { get; set; }

        public string OwnerUserId { get; set; }

        [Display(Name = "Problem text"), Required, AllowHtml]
        public string Body { get; set; }

        [Display(Name = "Answer option 1"), Required, AllowHtml]
        public string Option1 { get; set; }

        [Display(Name = "Answer option 2"), Required, AllowHtml]
        public string Option2 { get; set; }

        [Display(Name = "Answer option 3"), AllowHtml]
        public string Option3 { get; set; }
        
        [Display(Name = "Answer option 4"), AllowHtml]
        public string Option4 { get; set; }

        public OptionViewModel[] GetOptions()
        {
            return new[] {
                new OptionViewModel(1, Option1),
                new OptionViewModel(2, Option2),
                new OptionViewModel(3, Option3),
                new OptionViewModel(4, Option4)
            }.Where(opt => !string.IsNullOrWhiteSpace(opt.Option))
            .ToArray();
        }
        public IEnumerable<OptionViewModel> GetAllOptions()
        {
            return new[] {
                new OptionViewModel(1, Option1),
                new OptionViewModel(2, Option2),
                new OptionViewModel(3, Option3),
                new OptionViewModel(4, Option4)
            };
        }

        [Display(Name = "Correct option number")]
        public int IndexOfCorrectOption { get; set; }

        [Display(Name = "Comment"), AllowHtml]
        public string Comment { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreateAt { get; set; }
    }
}