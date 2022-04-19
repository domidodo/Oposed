using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OposedApi.Enum;
using OposedApi.Error;
using OposedApi.Models;
using OposedApi.Utilities;

namespace OposedApi.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class AuthAttribute : Attribute, IAsyncActionFilter
    {
        public UserRole Role { get; set; } = UserRole.Admin;
        private const string AUTHNAME = "AuthKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AUTHNAME, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    ErrorCode = ((int)Errors.AUTHKEY_NOT_FOUND) + 1,
                    ErrorMessage = "AuthKey not found"
                });
                return;
            }

            User usr = AuthenticationUtility.GetUserByAuthId(extractedApiKey);
            if (usr == null)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    ErrorCode = ((int)Errors.AUTHKEY_INVALID) + 1,
                    ErrorMessage = "AuthKey invalid"
                });
                return;
            }

            if (Role == UserRole.Admin && usr.Role != UserRole.Admin)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    ErrorCode = ((int)Errors.PERMISSIONS_FAILED) + 1,
                    ErrorMessage = "Permissions fail"
                });
                return;
            }

            context.HttpContext.Items["User"] = usr;


            await next();
        }
    }
}
