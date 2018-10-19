using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using System.IO;

namespace SIS.MVCFramework.Routers
{
    public class ResourceRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            var path = request.Path;

            var lastDotIndex = path.LastIndexOf('.');
            var lastSlashIndex = path.LastIndexOf('/');
            var extension = path.Substring(lastDotIndex + 1);

            var resourceName = path.Substring(lastSlashIndex);

            var resourcePath = MvcContext.Get.RootDirectoryRelativePath +
                $"/{MvcContext.Get.ResourceFolderName}" +
                $"/{extension}" +
                resourceName;

            if (!File.Exists(resourcePath))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return new InlineResourceResult(File.ReadAllBytes(resourcePath), HttpResponseStatusCode.Ok);
        }
    }
}
