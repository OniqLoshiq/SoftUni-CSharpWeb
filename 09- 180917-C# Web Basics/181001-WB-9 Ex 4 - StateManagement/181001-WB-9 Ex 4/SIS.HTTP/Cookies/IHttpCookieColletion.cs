using System.Collections.Generic;

namespace SIS.HTTP.Cookies
{
    public interface IHttpCookieColletion : IEnumerable<HttpCookie>
    {
        void Add(HttpCookie cookie);

        bool ContainsCookie(string key);

        HttpCookie GetCookie(string key);

        bool HasCookies();
    }
}
