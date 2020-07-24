using System.ComponentModel.DataAnnotations;

namespace SpamDetector.Models
{
    public class Comment
    {
        [Required]
        [MinLength(1)]
        [MaxLength(1500)]
        public string Text { get; set; }

        public bool AllowEmails { get; set; }

        public bool AllowLinks { get; set; }

        public bool AllowPhoneNumbers { get; set; }
    }
}