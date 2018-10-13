using SIS.HTTP.Common;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace SIS_IRunes.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if(this.IsAuthenticated(request))
            {
                string cookieData = request.Cookies.GetCookie(GlobalConstants.AuthenticationCookieKey).Value;
                string username = this.UserCookieService.GetUserData(cookieData);

                this.ViewBag["username"] = username;

                return this.View("IndexLoggedIn", true);
            }

            return this.View("Index", false);
        }
    }
}
