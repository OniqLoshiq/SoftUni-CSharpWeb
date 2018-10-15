using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using System;
using System.IO;
using System.Linq;

namespace SIS.WebServer.Api
{
    public class HttpHandler : IHttpHandler
    {
        private const string RootDirRelativePath = "../../../";

        private const string ResourceFolderName = "Resources";

        private const string DirSeparator = "/";

        private ServerRoutingTable serverRoutingTable;

        public HttpHandler(ServerRoutingTable serverRoutingTable)
        {
            this.serverRoutingTable = serverRoutingTable;
        }

        public IHttpResponse Handle(IHttpRequest request)
        {
            var isResourceRequest = this.IsResourceRequest(request);

            if (isResourceRequest)
            {
                return this.HandleResourceRequest(request.Path);
            }

            if (!this.serverRoutingTable.Routes.ContainsKey(request.RequestMethod)
                || !this.serverRoutingTable.Routes[request.RequestMethod].ContainsKey(request.Path))
            {
                return new BadRequestResult($"Path not found!", HttpResponseStatusCode.NotFound);
            }

            return this.serverRoutingTable.Routes[request.RequestMethod][request.Path].Invoke(request);
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

        private IHttpResponse HandleResourceRequest(string path)
        {
            var lastDotIndex = path.LastIndexOf('.');
            var lastSlashIndex = path.LastIndexOf('/');
            var extension = path.Substring(lastDotIndex + 1);

            var resourceName = path.Substring(lastSlashIndex);

            var resourcePath = RootDirRelativePath +
                ResourceFolderName +
                DirSeparator +
                extension +
                resourceName;

            if (!File.Exists(resourcePath))
            {
                return new BadRequestResult($"Path not found!", HttpResponseStatusCode.NotFound);
            }

            return new InlineResourceResult(File.ReadAllBytes(resourcePath), HttpResponseStatusCode.Ok);
        }
    }
}
