using AdOut.Extensions.Authorization;
using AdOut.Extensions.Repositories;
using AdOut.Planning.Model;
using AdOut.Planning.Model.Interfaces.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AdOut.Planning.WebApi.Filters
{
    public class ResourceAuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        private readonly Type _resourceType;
        private readonly string _resourceIdName;

        public ResourceAuthorizationAttribute(Type resourceType, string resourceIdName = "id")
        {
            _resourceType = resourceType;
            _resourceIdName = resourceIdName;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
                return;
            }

            var actionDesc = (ControllerActionDescriptor)context.ActionDescriptor;
            var resourceIdName = FindResourceIdName(actionDesc.MethodInfo.GetParameters());
            resourceIdName = !string.IsNullOrEmpty(resourceIdName) ? resourceIdName : _resourceIdName;         
            var resourceId = GetResourceId(context.ActionArguments, resourceIdName);

            var dbcontext = context.HttpContext.RequestServices.GetRequiredService<IDatabaseContext>();
            var resource = await dbcontext.FindAsync(_resourceType, resourceId);
            if (resource == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            var persistentResource = (PersistentEntity)resource;
            var userId = user.FindFirst(ClaimTypeNames.UserId).Value;
            if (persistentResource.Creator != userId)
            {
                context.Result = new ForbidResult();
            }

            await next();
        }

        private string FindResourceIdName(ParameterInfo[] parameters)
        {
            foreach (var param in parameters)
            {
                if (param.GetCustomAttribute(typeof(ResourceIdAttribute)) != null)
                {
                    return param.Name;
                }

                var prop = param.ParameterType.GetProperties().FirstOrDefault(prop =>
                {
                    var attr = prop.GetCustomAttribute(typeof(ResourceIdAttribute));
                    return attr != null;
                });

                if (prop != null)
                {
                    return prop.Name;
                }      
            }
            return null;
        }

        private object GetResourceId(IDictionary<string, object> ActionArguments, string propName)
        {
            foreach (var arg in ActionArguments)
            {
                if (arg.Key == propName)
                {
                    return arg.Value;      
                }

                var prop = arg.Value.GetType().GetProperty(propName);
                if (prop != null)
                {
                    return prop.GetValue(arg.Value);
                }               
            }
            return null;
        }
    }
}
