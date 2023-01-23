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
    public class TemplateController : ControllerBase
    {
        [HttpGet]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Get all template for current user")]
        public ActionResult<List<Template>> GetAll()
        {
            var currentUser = UserUtility.GetCurrentUser(HttpContext);
            return TemplateUtility.GetTemplatesByUserId(currentUser.Id);
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get templae by id")]
        public ActionResult<Template> GetTemplate(int id)
        {
            var currentUser = UserUtility.GetCurrentUser(HttpContext);
            var template = TemplateUtility.GetTemplateById(id);

            if (template != null && (template.UserId == currentUser.Id || template.UserId == 0))
                return template;
            else
                return ErrorManager.Get(Errors.TEMPLATE_NOT_FOUND);
        }

        [HttpPut]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Update or Instert Templates")]
        public ActionResult Update(Template t)
        {
            var currentUser = UserUtility.GetCurrentUser(HttpContext);
            if (currentUser.Role == UserRole.User && t.IsPublic) {
                return ErrorManager.Get(Errors.TEMPLATE_UPDATING_FAILED);
            }

            t.UserId = t.IsPublic ? 0 : currentUser.Id;
            var successful = TemplateUtility.SaveTemplate(t);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.TEMPLATE_UPDATING_FAILED);
        }
    }
}
