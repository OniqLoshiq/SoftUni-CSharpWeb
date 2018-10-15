using SIS.MVCFramework.ActionResults.Contracts;

namespace SIS.MVCFramework.ActionResults
{
    public class RedirectResult : IRedirectable
    {
        public RedirectResult(string redirectUrl)
        {
            this.RedirectUrl = redirectUrl;
        }

        public string RedirectUrl { get; }

        public string Invoke() => this.RedirectUrl;
    }
}
