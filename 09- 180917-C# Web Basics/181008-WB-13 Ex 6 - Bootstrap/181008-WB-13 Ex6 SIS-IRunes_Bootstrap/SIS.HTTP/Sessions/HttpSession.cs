using System.Collections.Generic;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
        private readonly IDictionary<string, object> sessionParameters;

        public HttpSession(string id)
        {
            this.Id = id;
            this.sessionParameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public void AddParameter(string name, object parameter)
        {
            this.sessionParameters[name] = parameter;
        }

        public void ClearParameters()
        {
            this.sessionParameters.Clear();
        }

        public bool ContainsParameter(string name)
        {
            return this.sessionParameters.ContainsKey(name);
        }

        public object GetParameter(string name)
        {
            if (!this.ContainsParameter(name))
            {
                return null;
            }

            return this.sessionParameters[name];
        }
    }
}