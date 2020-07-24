using SpamDetector.Models;
using SpamDetector.Services;
using Xunit;

namespace Tests
{
    public class SpamDetectorTests
    {
        private readonly SpamDetectorService _spamDetectorService;

        public SpamDetectorTests()
        {
            _spamDetectorService = new SpamDetectorService();
        }
        
        [Theory]
        [InlineData("This is a simple model comment.\n I live at 45th Darwin Street @ St. Lewis.")]
        [InlineData("hello! this is, yet another, model comment to test success.")]
        public void IsSpam_Test_NoAllow_Success(string text)
        {
            var result = _spamDetectorService.IsSpam(new Comment {Text = text});
            Assert.False(result);
        }
        
        [Theory]
        [InlineData("Check out this link http://winiphone.com")]
        [InlineData("Download from this link below\n ftp://dontdownload.com/program.exe")]
        [InlineData("Visit our site for exclusive offers!! Check now -> https://exclusiveoffers.com")]
        [InlineData("Win a free iPhone from our site at www.iphonefreeforeveryone.net")]
        public void IsSpam_Test_AllowLinks_Success(string text)
        {
            var result = _spamDetectorService.IsSpam(new Comment {Text = text, AllowLinks = true});
            Assert.False(result);
        }
        
        [Theory]
        [InlineData("Mail me at email@gmail.com")]
        [InlineData("for special exclusive offers, contact us at contact+offers@site.com")]
        public void IsSpam_Test_AllowEmails_Success(string text)
        {
            var result = _spamDetectorService.IsSpam(new Comment {Text = text, AllowEmails = true});
            Assert.False(result);
        }
        
        [Theory]
        [InlineData("Contact us at +1 789-888-8285")]
        [InlineData("you can find me on WhatsApp on +91 1234567890")]
        [InlineData("Use 4949990505 to call us today!")]
        public void IsSpam_Test_AllowPhoneNumbers_Success(string text)
        {
            var result = _spamDetectorService.IsSpam(new Comment {Text = text, AllowPhoneNumbers = true});
            Assert.False(result);
        }
        
        [Theory]
        [InlineData("Check out this link http://winiphone.com")]
        [InlineData("Download from this link below\n ftp://dontdownload.com/program.exe")]
        [InlineData("Visit our site for exclusive offers!! Check now -> https://exclusiveoffers.com")]
        [InlineData("Win a free iPhone from our site at www.iphonefreeforeveryone.net")]
        public void IsSpam_Test_Links_Failure(string text)
        {
            var result = _spamDetectorService.IsSpam(new Comment {Text = text});
            Assert.True(result);
        }

        [Theory]
        [InlineData("Mail me at email@gmail.com")]
        [InlineData("for special exclusive offers, contact us at contact+offers@site.com")]
        public void IsSpam_Test_Emails_Failure(string text)
        {
            var result = _spamDetectorService.IsSpam(new Comment {Text = text});
            Assert.True(result);
        }
        
        [Theory]
        [InlineData("Contact us at +1 789-888-8285")]
        [InlineData("you can find me on WhatsApp on +91 1234567890")]
        [InlineData("Use 4949990505 to call us today!")]
        public void IsSpam_Test_PhoneNumbers_Failure(string text)
        {
            var result = _spamDetectorService.IsSpam(new Comment {Text = text});
            Assert.True(result);
        }
    }
}