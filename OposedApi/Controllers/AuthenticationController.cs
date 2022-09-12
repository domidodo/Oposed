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

            if (!AuthenticationUtility.CheckPassword(user, auth.Password))
                return ErrorManager.Get(Errors.USER_INVALID_PASSWORD);

            if (!user.Active)
                return ErrorManager.Get(Errors.USER_BLOCKED);

            if (user.PasswordExpirationDate < DateTime.Now)
                return ErrorManager.Get(Errors.USER_EXPIRATED_PASSWORD);
            
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
