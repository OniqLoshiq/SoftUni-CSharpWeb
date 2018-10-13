using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.HTTP.Sessions;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIS.WebServer
{
    public class ConnectionHandler
    {
        private const string RootDirRelativePath = "../../../";

        private const string ResourceFolderName = "Resources";

        private const string DirSeparator = "/";
        
        private readonly Socket client;

        private readonly ServerRoutingTable serverRoutingTable;

        public ConnectionHandler(Socket client, ServerRoutingTable serverRoutingTable)
        {
            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            var isResourceRequest = this.IsResourceRequest(httpRequest);

            if(isResourceRequest)
            {
                return this.HandleResourceRequest(httpRequest.Path);
            }

            if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod)
                || !this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path))
            {
                return new BadRequestResult($"Path not found!", HttpResponseStatusCode.NotFound);
            }

            return this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
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

            if(!File.Exists(resourcePath))
            {
                return new BadRequestResult($"Path not found!", HttpResponseStatusCode.NotFound);
            }

            return new InlineResourceResult(File.ReadAllBytes(resourcePath), HttpResponseStatusCode.Ok);
        }

        private bool IsResourceRequest(IHttpRequest httpRequest)
        {
            var requestPath = httpRequest.Path;

            if(requestPath.Contains('.'))
            {
                var lastDotIndex = requestPath.LastIndexOf('.');
                var extension = requestPath.Substring(lastDotIndex);

                var resourceExtension = GlobalConstants.ResourceExtensions.Contains(extension);

                return resourceExtension;
            }

            return false;
        }

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            await this.client.SendAsync(byteSegments, SocketFlags.None);
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await this.ReadRequest();

            if (httpRequest != null)
            {
                var httpResponse = this.HandleRequest(httpRequest);

                string sessionId = this.SetSession(httpRequest, httpResponse);
                
                await this.PrepareResponse(httpResponse);
            }

            this.client.Shutdown(SocketShutdown.Both);
        }

        //Adding session cookie to response only when there is no request session cookie
        private string SetSession(IHttpRequest httpRequest, IHttpResponse httpResponse)
        {
            string sessionId = null;

            if (httpRequest.Cookies.ContainsCookie(GlobalConstants.SessionCookieKey))
            {
                var cookie = httpRequest.Cookies.GetCookie(GlobalConstants.SessionCookieKey);
                sessionId = cookie.Value;

                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);

                httpResponse.AddCookie(new HttpCookie(GlobalConstants.SessionCookieKey, $"{sessionId}"));
            }

            return sessionId;
        }
    }
}
