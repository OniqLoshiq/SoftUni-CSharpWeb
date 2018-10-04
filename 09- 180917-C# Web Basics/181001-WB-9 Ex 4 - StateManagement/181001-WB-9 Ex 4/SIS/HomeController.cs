using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;

namespace SIS
{
    public class HomeController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            string content = "<h1>Hello, Friend! How you doin'? Have a nice time!</h1>";

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}
