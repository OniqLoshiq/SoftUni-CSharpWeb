using SIS.HTTP.Common;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Contracts;
using System;
using System.Linq;

namespace SIS.MVCFramework.Routers
{
    public class HttpRouteHandlingContext : IHttpHandlingContext
    {
        public HttpRouteHandlingContext(IHttpHandler controllerHandler, IHttpHandler resourceHandler)
        {
            this.ControllerHandler = controllerHandler;
            this.ResourceHandler = resourceHandler;
        }

        protected IHttpHandler ControllerHandler { get; }

        protected IHttpHandler ResourceHandler { get; }

        public IHttpResponse Handle(IHttpRequest request)
        {
            if (this.IsResourceRequest(request))
            {
                return this.ResourceHandler.Handle(request);
            }

            return this.ControllerHandler.Handle(request);
        }

        private bool IsResourceRequest(IHttpRequest request)
        {
            var requestPath = request.Path;

            if (requestPath.Contains('.'))
            {
                var lastDotIndex = requestPath.LastIndexOf('.');
                var extension = requestPath.Substring(lastDotIndex);

                var resourceExtension = GlobalConstants.ResourceExtensions.Contains(extension);

                return resourceExtension;
            }

            return false;
        }
    }
}
