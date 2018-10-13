using IRunesData;
using IRunesServices;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System.Collections.Generic;
using System.IO;

namespace SIS_IRunes.Controllers
{
    public abstract class BaseController
    {
        private const string RootDirRelativePath = "../../../";

        private const string ViewsFolderName = "Views";

        private const string HtmlFileExtension = ".html";

        private const string DirSeparator = "/";

        private const string LoggedOutLayoutName = "_LayoutLoggedOut";

        private const string LoggedInLayoutName = "_LayoutLoggedIn";

        private const string RenderBodyContentValue = "{{renderBody}}";

        private string GetCurrentControllerName() => this.GetType().Name.Replace("Controller", string.Empty);

        protected IDictionary<string, string> ViewBag { get; set; }

        protected IRunesDbContext DbContext { get; set; }

        protected IHashService HashService { get; set; }

        protected IUserCookieService UserCookieService { get; set; }

        public BaseController()
        {
            this.DbContext = new IRunesDbContext();
            this.HashService = new HashService();
            this.UserCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        public bool IsAuthenticated(IHttpRequest request)
        {
            return request.Cookies.ContainsCookie(GlobalConstants.AuthenticationCookieKey);
        }

        protected IHttpResponse View(string viewName, bool isLogged)
        {
            string layoutName = isLogged ? LoggedInLayoutName : LoggedOutLayoutName;

            string layoutPath = RootDirRelativePath +
                ViewsFolderName +
                DirSeparator +
                layoutName +
                HtmlFileExtension;

            string filePath = RootDirRelativePath +
               ViewsFolderName +
               DirSeparator +
               this.GetCurrentControllerName() +
               DirSeparator +
               viewName +
               HtmlFileExtension;

            if (!File.Exists(filePath))
            {
                return new BadRequestResult($"{viewName} not found!", HttpResponseStatusCode.NotFound);
            }

            var bodyContent = File.ReadAllText(filePath);

            foreach (var viewBagKey in this.ViewBag.Keys)
            {
                var dynamicPlaceholder = $"{{{{{viewBagKey}}}}}";

                if (bodyContent.Contains(dynamicPlaceholder))
                {
                    bodyContent = bodyContent.Replace(dynamicPlaceholder, this.ViewBag[viewBagKey]);
                }
            }

            var layoutContent = File.ReadAllText(layoutPath);

            var viewContent = layoutContent.Replace(RenderBodyContentValue, bodyContent);

            var response = new HtmlResult(viewContent, HttpResponseStatusCode.Ok);

            return response;
        }
    }
}
