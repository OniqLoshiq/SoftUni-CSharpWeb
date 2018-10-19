using System;
using System.Collections.Generic;
using System.Linq;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;
using SIS.HTTP.Sessions;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpCookieColletion Cookies { get; }

        public IHttpSession Session { get; set; }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            string[] requestLine = splitRequestContent[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            if (requestLine.Length != 3 || requestLine[2] != GlobalConstants.HttpOneProtocolFragment)
                return false;

            string method = requestLine[0].Capitalize();
            Enum.TryParse(typeof(HttpRequestMethod), method, out object result);
            if (result == null)
                return false;

            return true;
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            if (string.IsNullOrWhiteSpace(queryString))
                return false;

            return queryParameters.Length >= 1;
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            this.RequestMethod = Enum.Parse<HttpRequestMethod>(requestLine[0], true);
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            this.Url = requestLine[1];
        }

        private void ParseRequestPath()
        {
            this.Path = this.Url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private void ParseHeaders(string[] requestContent)
        {
            if (!requestContent.Any())
            {
                throw new BadRequestException();
            }

            int emptyLineIndex = Array.IndexOf(requestContent, string.Empty);
            string[] headersData = requestContent.Take(emptyLineIndex).ToArray();

            bool hasHostHeader = false;

            foreach (var header in headersData)
            {
                if (header.IndexOf(GlobalConstants.HostHeaderKey) == 0)
                {
                    hasHostHeader = true;
                    break;
                }
            }

            if (!hasHostHeader)
            {
                throw new BadRequestException();
            }

            foreach (var header in headersData)
            {
                string[] headerInfo = header.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

                var httpHeader = new HttpHeader(headerInfo[0], headerInfo[1]);

                this.Headers.Add(httpHeader);
            }
        }

        private void ParseCookies()
        {
            var cookieData = this.Headers.GetHeader(GlobalConstants.CookieHeaderKey);

            if (cookieData != null)
            {
                var cookieKeyValuePairs = cookieData.Value.Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var keyValueData in cookieKeyValuePairs)
                {
                    var keyValue = keyValueData.Split(new[] { '=' }, 2);

                    if (keyValue.Length != 2)
                        continue;

                    this.Cookies.Add(new HttpCookie(keyValue[0], keyValue[1], false));
                }
            }
        }

        private void ParseQueryParameters()
        {
            string queryString = this.Url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[1];

            if(string.IsNullOrWhiteSpace(queryString))
            {
                return;
            }

            string[] queryParameters = queryString.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

            if (!this.IsValidRequestQueryString(queryString, queryParameters))
            {
                throw new BadRequestException();
            }

            foreach (var keyValuePairData in queryParameters)
            {
                var keyValuePair = keyValuePairData.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                this.QueryData[keyValuePair[0]] = keyValuePair[1];
            }
        }

        private void ParseFormDataParameters(string formData)
        {
            if(!formData.Contains('='))
            {
                throw new BadRequestException();
            }

            string[] formParams = formData.Split(new[] { '&' });

            foreach (var keyValuePairData in formParams)
            {
                string[] keyValuePair = keyValuePairData.Split(new[] { '=' }, 2);

                this.FormData[keyValuePair[0]] = keyValuePair[1];
            }
        }

        private void ParseRequestParameters(string formData)
        {
            if (this.Url.IndexOf('?') >= 0)
            {
                this.ParseQueryParameters();
            }

            if(!string.IsNullOrWhiteSpace(formData))
            {
                this.ParseFormDataParameters(formData);
            }
        }
    }
}
