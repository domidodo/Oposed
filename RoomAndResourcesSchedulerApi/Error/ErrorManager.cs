using Microsoft.AspNetCore.Mvc;

namespace RoomAndResourcesSchedulerApi.Error
{
    public static class ErrorManager
    {
        public static BadRequestObjectResult Get(Errors error)
        {
            string errorMessage = "Unknown error";

            switch (error) {
                case Errors.USER_NOT_FOUND:
                    errorMessage = "User not found";
                    break;
                case Errors.USER_BLOCKED:
                    errorMessage = "User is blocked";
                    break;
                case Errors.USER_INVALID_PASSWORD:
                    errorMessage = "Password is invalid";
                    break;
                case Errors.AUTHKEY_NOT_FOUND:
                    errorMessage = "AuthKey not found";
                    break;
                case Errors.AUTHKEY_INVALID:
                    errorMessage = "AuthKey is invalid";
                    break;
                case Errors.PERMISSIONS_FAILED:
                    errorMessage = "Permission failed";
                    break;
                case Errors.RESOURCE_NOT_FOUND:
                    errorMessage = "Resource not found";
                    break;
                case Errors.RESOURCE_DELETING_FAILED:
                case Errors.EVENT_DELETING_FAILED:
                    errorMessage = "Deleting failed";
                    break;
                case Errors.RESOURCE_UPDATING_FAILED:
                case Errors.EVENT_UPDATING_FAILED:
                    errorMessage = "Updating failed";
                    break;
                case Errors.EVENT_INSERT_FAILED:
                    errorMessage = "Insertind failed";
                    break;
                case Errors.EVENT_NOT_FOUND:
                    errorMessage = "Event not found";
                    break;
            }

            return new BadRequestObjectResult(new
            {
                ErrorId = ((int)error)+1,
                ErrorCode = error.ToString(),
                ErrorMessage = errorMessage
            });
        }
    }
}
