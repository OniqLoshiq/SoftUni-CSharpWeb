using SIS.HTTP.Requests;
using SIS.MVCFramework.ActionResults;
using SIS.MVCFramework.ActionResults.Contracts;
using SIS.MVCFramework.Utilities;
using SIS.MVCFramework.Views;
using System.Runtime.CompilerServices;

namespace SIS.MVCFramework.Controllers
{
    public abstract class Controller
    {
        public IHttpRequest Request { get; set; }

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            var contollerName = ControllerUtilities.GetContollerName(this);

            var fullyQuailifiedName = ControllerUtilities.GetViewFullQualifiedName(contollerName, viewName);

            var view = new View(fullyQuailifiedName);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);
    }
}
