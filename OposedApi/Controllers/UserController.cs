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
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("Current")]
        [SwaggerOperation(Summary = "Get current user")]
        public ActionResult<User> GetCurrentUser()
        {
            return UserUtility.GetCurrentUser(HttpContext);
        }

        [HttpGet]
        [Auth(Role = UserRole.Admin)]
        [SwaggerOperation(Summary = "Get all users")]
        public ActionResult<List<User>> GetAllUsers()
        {
            return UserUtility.GetAllUsers();
        }


        [HttpGet]
        [Auth(Role = UserRole.Admin)]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get user by id")]
        public ActionResult<User> GetUser(int id)
        {
            User usr = UserUtility.GetUser(id);

            if(usr != null)
                return UserUtility.GetUser(id);
            else
                return ErrorManager.Get(Errors.USER_NOT_FOUND);
        }
        

        [HttpPut]
        [Auth(Role = UserRole.Admin)]
        [Route("Refetch/{id}")]
        [SwaggerOperation(Summary = "Refetch userdata from LDAP by user-id")]
        public ActionResult RefetchUserData(int id)
        {
            User usr = UserUtility.GetUser(id);
            usr = UserUtility.Refetch(usr);

            if (usr != null)
                return Ok();
            else
                return ErrorManager.Get(Errors.USER_NOT_FOUND);
        }
    }
}
