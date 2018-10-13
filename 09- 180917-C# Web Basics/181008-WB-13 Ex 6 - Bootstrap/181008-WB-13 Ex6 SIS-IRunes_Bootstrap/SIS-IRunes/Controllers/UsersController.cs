using IRunesModels;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Linq;
using System.Net;

namespace SIS_IRunes.Controllers
{
    public class UsersController : BaseController
    {
        //Not allowing logged user to access this route by typing it manualy
        public IHttpResponse LoginGet(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return new BadRequestResult("You are already logged in!!!", SIS.HTTP.Enums.HttpResponseStatusCode.Forbidden);
            }

            return this.View("LoginGet", false);
        }

        public IHttpResponse LoginPost(IHttpRequest request)
        {
            string loginCredentials = request.FormData["loginCredentials"].ToString();
            string password = request.FormData["password"].ToString();

            if (string.IsNullOrWhiteSpace(loginCredentials) || string.IsNullOrWhiteSpace(password))
            {
                return new RedirectResult("/Users/Login");
            }

            string hashedPassword = this.HashService.Hash(password);

            string username = string.Empty;
            string email = string.Empty;

            User user = null;

            if (loginCredentials.Contains('@'))
            {
                email = loginCredentials;
                user = this.DbContext.Users.FirstOrDefault(u => u.Email == email && u.HashedPassword == hashedPassword);
            }
            else
            {
                username = loginCredentials;
                user = this.DbContext.Users.FirstOrDefault(u => u.Username == username && u.HashedPassword == hashedPassword);
            }

            if (user == null)
            {
                return new RedirectResult("/Users/Login");
            }

            return SingnInUser(request, user);
        }

        //Not allowing logged user to access this route by typing it manualy
        public IHttpResponse RegisterGet(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return new BadRequestResult("You are already logged in!!!", SIS.HTTP.Enums.HttpResponseStatusCode.Forbidden);
            }

            return this.View("RegisterGet", false);
        }

        public IHttpResponse RegisterPost(IHttpRequest request)
        {
            string username = request.FormData["username"].ToString().Trim();
            string password = request.FormData["password"].ToString();
            string confirmedPassword = request.FormData["confirmedPassword"].ToString();
            string email = WebUtility.UrlDecode(request.FormData["email"].ToString());

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmedPassword) ||
                string.IsNullOrWhiteSpace(email) ||
                password != confirmedPassword ||
                !email.Contains('@') ||
                this.DbContext.Users.Any(u => u.Username == username) ||
                this.DbContext.Users.Any(u => u.Email == email))
            {
                return new RedirectResult("/Users/Register");
            }

            string hashedPassword = this.HashService.Hash(password);

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                HashedPassword = hashedPassword,
                Email = email
            };

            this.DbContext.Users.Add(user);

            try
            {
                this.DbContext.SaveChanges();
            }
            catch (Exception)
            {
                return new BadRequestResult("Something went wrong :)...", SIS.HTTP.Enums.HttpResponseStatusCode.InternalServerError);
            }

            return SingnInUser(request, user);
        }

        private IHttpResponse SingnInUser(IHttpRequest request, User user)
        {
            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var response = new RedirectResult("/Home/Index");
            var cookie = new HttpCookie(GlobalConstants.AuthenticationCookieKey, cookieContent);
            response.Cookies.Add(cookie);

            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            var authenticatedCookie = request.Cookies.GetCookie(GlobalConstants.AuthenticationCookieKey);
            authenticatedCookie.Delete();

            var response = new RedirectResult("/Home/Index");
            response.Cookies.Add(authenticatedCookie);

            return response;
        }
    }
}
