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
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        [SwaggerOperation(Summary = "Login with mail and password and get the authkey")]
        public ActionResult<User> Login(Authentication auth)
        {
            var user = AuthenticationUtility.GetUserByAuthentication(auth);
            
            if (user == null)
                return ErrorManager.Get(Errors.USER_NOT_FOUND);

            UserUtility.Refetch(user);
            
            if (!user.Active)
                return ErrorManager.Get(Errors.USER_BLOCKED);
             
            if (!AuthenticationUtility.CheckPassword(user, auth.Password))
                return ErrorManager.Get(Errors.USER_INVALID_PASSWORD);

            user.AuthKey = AuthenticationUtility.GetNewAuthId();
            user.LastLogin = DateTime.Now;

            UserUtility.SaveUser(user);

            return user;
        }

        [HttpPut]
        [Route("Logout")]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Logout")]
        public ActionResult Logout()
        {
            this.HttpContext.Request.Headers.TryGetValue("AuthKey", out var authKey);

            var user = AuthenticationUtility.GetUserByAuthId(authKey);
            user.AuthKey = null;
            UserUtility.SaveUser(user);

            return Ok();
        }
    }
}
