using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MVCFramework.ActionResults.Contracts;
using SIS.MVCFramework.Attributes.Methods;
using SIS.MVCFramework.Controllers;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIS.MVCFramework.Routers
{
    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var controllerName = string.Empty;
            var actionName = string.Empty;

            if (request.Path != "/")
            {
                var requestPath = request.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                controllerName = CapitalizeFirstLetter(requestPath[0].ToLower());
                actionName = CapitalizeFirstLetter(requestPath[1].ToLower());
            }
            else
            {
                controllerName = "Home";
                actionName = "Index";
            }

            var requestMethod = request.RequestMethod.ToString();

            var controller = this.GetController(controllerName, request);

            var action = this.GetAction(requestMethod, controller, actionName);

            if(controller == null || action == null)
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.PrepareResponse(controller, action);
        }

        private string CapitalizeFirstLetter(string s)
        {
            return s.First().ToString().ToUpper() + s.Substring(1);
        }

        private IHttpResponse PrepareResponse(Controller controller, MethodInfo action)
        {
            IActionResult actionResult = (IActionResult)action.Invoke(controller, null);
            string invocationResult = actionResult.Invoke();

            if(actionResult is IViewable)
            {
                return new HtmlResult(invocationResult, HttpResponseStatusCode.Ok);
            }
            else if(actionResult is IRedirectable)
            {
                return new RedirectResult(invocationResult);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supported.");
            }
        }

        private MethodInfo GetAction(string requestMethod, Controller controller, string actionName)
        {
            var actions = this.GetSuitableMethods(controller, actionName);

            if(!actions.Any())
            {
                return null;
            }

            foreach (var methodInfo in actions)
            {
                var attributes = methodInfo.GetCustomAttributes().Where(a => a is HttpMethodAttribute).Cast<HttpMethodAttribute>();

                if(!attributes.Any() && requestMethod.ToUpper() == "GET")
                {
                    return methodInfo;
                }

                foreach (var attribute in attributes)
                {
                    if(attribute.IsValid(requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if(controller == null)
            {
                return new MethodInfo[0];
            }

            return controller.GetType().GetMethods().Where(mi => mi.Name.ToLower() == actionName.ToLower());
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            if(!string.IsNullOrWhiteSpace(controllerName))
            {
                var controllerTypeName = string.Format("{0}.{1}.{2}{3}, {0}",
                    MvcContext.Get.AssemblyName,
                    MvcContext.Get.ControllersFolder, 
                    controllerName,
                    MvcContext.Get.ControllersSuffix);
              
                var controllerType = Type.GetType(controllerTypeName);
                var controller = (Controller)Activator.CreateInstance(controllerType);

                if(controller != null)
                {
                    controller.Request = request;
                }

                return controller;
            }

            return null;
        }
    }
}
