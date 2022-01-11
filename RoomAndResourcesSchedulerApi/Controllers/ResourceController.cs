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
    public class ResourceController : ControllerBase
    {
         
        [HttpGet]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Get all Resources")]
        public ActionResult<List<Resource>> GetAllResources()
        {
            return ResourceUtility.GetAllResources();
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get Resource by id")]
        public ActionResult<Resource> GetResource(int id)
        {
            Resource Resource = ResourceUtility.GetResourceById(id);
            if (Resource != null)
                return Resource;
            else
                return ErrorManager.Get(Errors.RESOURCE_NOT_FOUND); 
        }

        [HttpPut]
        [Auth(Role = UserRole.Admin)]
        [SwaggerOperation(Summary = "Update a Resource")]
        public ActionResult UpdateResource(Resource resource)
        {
            var successful = ResourceUtility.UpdateResource(resource);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.RESOURCE_UPDATING_FAILED);
        }


        [HttpPost]
        [Auth(Role = UserRole.Admin)]
        [SwaggerOperation(Summary = "Add a new Resource")]
        public ActionResult<Resource> AddResource(Resource resource)
        {
            resource.Id = ResourceUtility.SaveResource(resource);
            return resource;
        }

 
        [HttpDelete]
        [Auth(Role = UserRole.Admin)]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a Resource by id")]
        public ActionResult DeleteResource(int id)
        {
            var successful = ResourceUtility.DeleteResourceById(id);
            if(successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.RESOURCE_DELETING_FAILED);
        }

    }
}
