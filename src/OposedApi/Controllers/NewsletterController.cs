using Microsoft.AspNetCore.Mvc;
using OposedApi.Attributes;
using OposedApi.Enum;
using OposedApi.Error;
using OposedApi.Models;
using OposedApi.Utilities;
using Swashbuckle.AspNetCore.Annotations;

namespace OposedApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsletterController : ControllerBase
    {
        [HttpGet]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Get all tags")]
        [Route("tags")]
        public ActionResult<List<string>> GetAllTags()
        {
            return NewsletterUtility.GetAllTags();
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Get all newsletter-settings")]
        public ActionResult<List<Newsletter>> GetAllNewsletter()
        {
            return NewsletterUtility.GetAllNewsletterSettings();
        }


        [HttpPost]
        [Auth(Role = UserRole.Admin)]
        [SwaggerOperation(Summary = "Override newsletter <-> tags connection")]
        public ActionResult UpdateTags(List<Newsletter> tags)
        {
            NewsletterUtility.SaveTags(tags);
            return Ok();
        }

        [HttpPut]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "CurrentUser subscribe by tagId")]
        [Route("{newsletterId}/subscribe")]
        public ActionResult subscribe(int newsletterId)
        {
            var currentUser = UserUtility.GetCurrentUser(HttpContext);
            
            var successful = NewsletterUtility.AddNewsletterToUser(currentUser, newsletterId);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.NEWSLETTER_UPDATING_FAILED);
        }

        [HttpPut]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "CurrentUser unsubscribe by tagId")]
        [Route("{newsletterId}/unsubscribe")]
        public ActionResult unsubscribe(int newsletterId)
        {
            var currentUser = UserUtility.GetCurrentUser(HttpContext);

            var successful = NewsletterUtility.DeleteNewsletterToUser(currentUser, newsletterId);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.NEWSLETTER_UPDATING_FAILED);
        }
    }
}
