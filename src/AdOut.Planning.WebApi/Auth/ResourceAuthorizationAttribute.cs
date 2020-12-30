﻿using AdOut.Extensions.Authorization;
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

namespace AdOut.Planning.WebApi.Auth
{
    public class ResourceAuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        private readonly Type _resourceType;
        public ResourceAuthorizationAttribute(Type resourceType)
        {
            _resourceType = resourceType;
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
            var resourceIdName = GetResourceIdName(actionDesc.MethodInfo.GetParameters());
            if (string.IsNullOrEmpty(resourceIdName))
            {
                throw new ArgumentException("The action parameters don't contain an argument or a property marked ResourceIdAttribute.");
            }

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

        private string GetResourceIdName(ParameterInfo[] parameters)
        {
            var resourceIdName = string.Empty;
            foreach (var param in parameters)
            {
                if (param.GetCustomAttribute(typeof(ResourceIdAttribute)) != null)
                {
                    resourceIdName = param.Name;
                    break;
                }

                var prop = param.ParameterType.GetProperties().FirstOrDefault(prop =>
                {
                    var attr = prop.GetCustomAttribute(typeof(ResourceIdAttribute));
                    return attr != null;
                });

                if (prop != null)
                {
                    resourceIdName = prop.Name;
                    break;
                }      
            }
            return resourceIdName;
        }

        private object GetResourceId(IDictionary<string, object> ActionArguments, string propName)
        {
            object resourceId = null;
            foreach (var arg in ActionArguments)
            {
                if (arg.Key == propName)
                {
                    resourceId = arg.Value;
                    break;
                }

                var prop = arg.Value.GetType().GetProperty(propName);
                if (prop != null)
                {
                    resourceId = prop.GetValue(arg.Value);
                    break;
                }               
            }
            return resourceId;
        }
    }
}
