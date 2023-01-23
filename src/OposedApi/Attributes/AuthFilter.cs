using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace OposedApi.Attributes
{
    public class AuthFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

           
            var actionDescriptor = (ControllerActionDescriptor)context.ApiDescription.ActionDescriptor;

            AuthAttribute? authAttribut = actionDescriptor.ControllerTypeInfo.GetCustomAttributes<AuthAttribute>().FirstOrDefault();
            if (authAttribut == null)
                authAttribut = actionDescriptor.MethodInfo.GetCustomAttributes<AuthAttribute>().FirstOrDefault();

            if (authAttribut != null)
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "AuthKey",
                    Description = authAttribut.Role == Enum.UserRole.User ? "AuthKey of user or admin" : "AuthKey of admin",
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "string" },
                    Required = true
                });
            }
        }
    }
}
