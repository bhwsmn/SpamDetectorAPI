using System.Linq;
using System.Text.RegularExpressions;
using SpamDetector.Models;

namespace SpamDetector.Services
{
    public class SpamDetectorService
    {
        private readonly string LegalSpecialCharacters = " \n\r<>?,./;:'\"!@#$%^&*()[]{}/*-+\\|~";

        public bool IsSpam(Comment comment)
        {
            // If emails are allowed then links must also be allowed as all emails contain a link
            // Example: example@mail.site -> This email contains the link "mail.site"
            if (comment.AllowEmails)
            {
                comment.AllowLinks = true;
            }
            
            var isSpam = ContainsIllegalCharacters(comment.Text) ||
                         (ContainsLinks(comment.Text) && !comment.AllowLinks) ||
                         (ContainsEmails(comment.Text) && !comment.AllowEmails) ||
                         (ContainsPhoneNumbers(comment.Text) && !comment.AllowPhoneNumbers);

            return isSpam;
        }

        private bool ContainsIllegalCharacters(string text)
        {
            var containsIllegalCharacters = !text.All(c =>
                LegalSpecialCharacters.Contains(c) ||
                char.IsLetterOrDigit(c));

            return containsIllegalCharacters;
        }
        
        private bool ContainsLinks(string text)
        {
            var pattern = "(?:(?:https?|ftp):\\/\\/)?[\\w\\/\\-?=%.]+\\.[\\w\\/\\-?=%.]+";
            
            var matches = new Regex(pattern).Matches(text);
            var containsLinks = matches.Count > 0;

            return containsLinks;
        }
        
        private bool ContainsEmails(string text)
        {
            var pattern =
                "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";
                
            var matches = new Regex(pattern).Matches(text);
            var containsEmails  = matches.Count > 0;

            return containsEmails;
        }
        
        private bool ContainsPhoneNumbers(string text)
        {
            var pattern =
                "(?:(?:\\+?([1-9]|[0-9][0-9]|[0-9][0-9][0-9])\\s*(?:[.-]\\s*)?)?(?:\\(\\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\\s*\\)|([0-9][1-9]|[0-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\\s*(?:[.-]\\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\\s*(?:[.-]\\s*)?([0-9]{4})(?:\\s*(?:#|x\\.?|ext\\.?|extension)\\s*(\\d+))?";

            var matches = new Regex(pattern).Matches(text);
            var containsPhoneNumbers = matches.Count > 0;

            return containsPhoneNumbers;
        }
    }
}