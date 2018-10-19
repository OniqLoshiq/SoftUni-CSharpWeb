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
using System.ComponentModel.DataAnnotations;
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

            if (controller == null || action == null)
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            object[] actionParameters = this.MapActionParameters(action, request, controller);

            IActionResult actionResult = InvokeAction(controller, action, actionParameters);

            return this.PrepareResponse(actionResult);
        }

        private string CapitalizeFirstLetter(string s)
        {
            return s.First().ToString().ToUpper() + s.Substring(1);
        }

        private IHttpResponse PrepareResponse(IActionResult actionResult)
        {
            string invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocationResult, HttpResponseStatusCode.Ok);
            }
            else if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocationResult);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supported.");
            }
        }

        private IActionResult InvokeAction(Controller controller, MethodInfo action, object[] actionParameters)
        {
            return (IActionResult)action.Invoke(controller, actionParameters);
        }

        private object[] MapActionParameters(MethodInfo action, IHttpRequest request, Controller controller)
        {
            ParameterInfo[] actionParametersInfo = action.GetParameters();
            object[] mappedActionParameters = new object[actionParametersInfo.Length];

            for (int i = 0; i < actionParametersInfo.Length; i++)
            {
                ParameterInfo currentParameterInfo = actionParametersInfo[i];

                if (currentParameterInfo.ParameterType.IsPrimitive ||
                    currentParameterInfo.ParameterType == typeof(string))
                {
                    var mappedParameter = this.ProcessPrimitiveParameter(currentParameterInfo, request);
                    if (mappedParameter == null)
                        break;

                    mappedActionParameters[i] = mappedParameter;
                }
                else
                {
                    var bindingModel = this.ProcessBindingModelParameters(currentParameterInfo, request);

                    controller.ModelState.Isvalid = this.IsValidModel(bindingModel);

                    mappedActionParameters[i] = bindingModel;
                }
            }

            return mappedActionParameters;
        }

        private bool? IsValidModel(object bindingModel)
        {
            var validationProperties = bindingModel.GetType()
                                       .GetProperties()
                                       .Where(p => Attribute.IsDefined(p, typeof(ValidationAttribute)));

            foreach (var prop in validationProperties)
            {

                var propAttributes = prop.GetCustomAttributes()
                                     .Where(a => a is ValidationAttribute)
                                     .Cast<ValidationAttribute>()
                                     .ToList();

                var propValue = prop.GetValue(bindingModel);

                foreach (var attribute in propAttributes)
                {
                    if (!attribute.IsValid(propValue))
                    {
                        return false;
                    }
                } 
            }

            return true;
        }

        private object ProcessBindingModelParameters(ParameterInfo param, IHttpRequest request)
        {
            Type bindingModelType = param.ParameterType;

            var bindingModelInstance = Activator.CreateInstance(bindingModelType);
            var bindingModelProperties = bindingModelType.GetProperties();

            foreach (var prop in bindingModelProperties)
            {
                try
                {
                    //Works with primitive and string properties only
                    object value = this.GetParameterFromRequestData(request, prop.Name.ToLower());
                    prop.SetValue(bindingModelInstance, value);
                }
                catch
                {
                    Console.WriteLine($"The {prop.Name} field could not be mapped.");
                }
            }

            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }

        private object ProcessPrimitiveParameter(ParameterInfo param, IHttpRequest request)
        {
            object value = this.GetParameterFromRequestData(request, param.Name);

            if (value == null)
            {
                return value;
            }
            return Convert.ChangeType(value, param.ParameterType);
        }

        private object GetParameterFromRequestData(IHttpRequest request, string name)
        {
            if (request.QueryData.ContainsKey(name)) return request.QueryData[name];
            if (request.FormData.ContainsKey(name)) return request.FormData[name];

            return null;
        }

        private MethodInfo GetAction(string requestMethod, Controller controller, string actionName)
        {
            var actions = this.GetSuitableMethods(controller, actionName);

            if (!actions.Any())
            {
                return null;
            }

            foreach (var methodInfo in actions)
            {
                var attributes = methodInfo.GetCustomAttributes().Where(a => a is HttpMethodAttribute).Cast<HttpMethodAttribute>();

                if (!attributes.Any() && requestMethod.ToUpper() == "GET")
                {
                    return methodInfo;
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.IsValid(requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller.GetType().GetMethods().Where(mi => mi.Name.ToLower() == actionName.ToLower());
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            if (!string.IsNullOrWhiteSpace(controllerName))
            {
                var controllerTypeName = string.Format("{0}.{1}.{2}{3}, {0}",
                    MvcContext.Get.AssemblyName,
                    MvcContext.Get.ControllersFolder,
                    controllerName,
                    MvcContext.Get.ControllersSuffix);

                var controllerType = Type.GetType(controllerTypeName);
                var controller = (Controller)Activator.CreateInstance(controllerType);

                if (controller != null)
                {
                    controller.Request = request;
                }

                return controller;
            }

            return null;
        }
    }
}
