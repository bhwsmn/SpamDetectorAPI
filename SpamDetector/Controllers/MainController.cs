using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpamDetector.Models;
using SpamDetector.Services;

namespace SpamDetector.Controllers
{
    [ApiController]
    [Route("")]
    public class MainController : ControllerBase
    {
        private readonly SpamDetectorService _spamDetectorService;

        public MainController(SpamDetectorService spamDetectorService)
        {
            _spamDetectorService = spamDetectorService;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IsSpam(Comment comment)
        {
            var isSpam = _spamDetectorService.IsSpam(comment);

            return isSpam ? StatusCode(403) : Ok();
        }
    }
}