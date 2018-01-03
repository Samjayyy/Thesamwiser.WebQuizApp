using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizWebApp.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }

        [Index(name:"IX_User_Name", IsUnique = true)]
        [StringLength(100), Required]
        public string Name { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool IsAdmin { get; set; }

        public string Pass { get; set; }
    }
}