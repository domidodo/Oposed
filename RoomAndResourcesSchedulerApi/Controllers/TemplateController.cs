using Microsoft.AspNetCore.Mvc;
using RoomAndResourcesSchedulerApi.Attributes;
using RoomAndResourcesSchedulerApi.Enum;
using RoomAndResourcesSchedulerApi.Error;
using RoomAndResourcesSchedulerApi.Models;
using RoomAndResourcesSchedulerApi.Utilities;
using Swashbuckle.AspNetCore.Annotations;

namespace RoomAndResourcesSchedulerApi.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class TemplateController : ControllerBase
    {
        
       [HttpGet]
       [Auth(Role = UserRole.User)]
       [SwaggerOperation(Summary = "Get all templates")]
       public ActionResult<List<User>> GetAllTemplates()
       {
           return UserUtility.GetAllUsers();
       }
        
       [HttpPost]
       [Auth(Role = UserRole.Admin)]
       [SwaggerOperation(Summary = "Add template")]
       public ActionResult<Template> AddTemplate(Template tmpl)
       {
           var id = TemplateUtility.SaveTemplate(tmpl);
           tmpl.Id = id;
           return tmpl;
       }
       
       [HttpDelete]
       [Auth(Role = UserRole.Admin)]
       [Route("{id}")]
       [SwaggerOperation(Summary = "Delete template by id")]
       public ActionResult<List<User>> DeleteTemplateById(int id)
       {
           var successful = TemplateUtility.DeleteTemplateById(id);
           if (successful)
               return Ok();
           else
               return ErrorManager.Get(Errors.TEMPLATE_DELETING_FAILED);
       }

    }
}
