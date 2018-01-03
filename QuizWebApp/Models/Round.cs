using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QuizWebApp.Models
{
    public class Round
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public Round()
        {
            CreateAt = DateTime.UtcNow;
        }

        [Key]
        public int RoundId { get; set; }

        public int SortOrder { get; set; }

        public string OwnerUserId { get; set; }

        [Display(Name = "Round"), Required, AllowHtml]
        public string Title { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreateAt { get; set; }
    }
}