using SIS.HTTP.Requests;
using SIS.MVCFramework.ActionResults;
using SIS.MVCFramework.ActionResults.Contracts;
using SIS.MVCFramework.Models;
using SIS.MVCFramework.Utilities;
using SIS.MVCFramework.Views;
using System.Runtime.CompilerServices;

namespace SIS.MVCFramework.Controllers
{
    public abstract class Controller
    {
        protected Controller()
        {
            this.ViewModel = new ViewModel();
        }

        public IHttpRequest Request { get; set; }

        protected ViewModel ViewModel { get; set; }

        public Model ModelState { get; } = new Model();

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            var contollerName = ControllerUtilities.GetContollerName(this);

            var fullyQuailifiedName = ControllerUtilities.GetViewFullQualifiedName(contollerName, viewName);

            var view = new View(fullyQuailifiedName, this.ViewModel.Data);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);
    }
}
