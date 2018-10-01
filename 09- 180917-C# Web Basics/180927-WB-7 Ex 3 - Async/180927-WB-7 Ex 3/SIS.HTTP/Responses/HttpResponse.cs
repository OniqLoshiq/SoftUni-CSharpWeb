using System;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;

namespace SIS.HTTP.Responses
{
    public class HttpResponse : IHttpResponse
    {
        public HttpResponse() { }

        public HttpResponse(HttpResponseStatusCode statusCode)
        {
            this.Headers = new HttpHeaderCollection();
            this.Content = new byte[0];
            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public byte[] Content { get ; set ; }

        public void AddHeader(HttpHeader header)
        {
            this.Headers.Add(header);
        }

        public byte[] GetBytes()
        {
            byte[] responseLineAndHeaders = Encoding.UTF8.GetBytes(this.ToString());

            byte[] responseBytes = new byte[responseLineAndHeaders.Length + this.Content.Length];

            responseLineAndHeaders.CopyTo(responseBytes, 0);
            this.Content.CopyTo(responseBytes, responseLineAndHeaders.Length);

            return responseBytes;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"{GlobalConstants.HttpOneProtocolFragment} {HttpResponseStatusExtentions.GetResponseLine((int)this.StatusCode)}").Append(Environment.NewLine)
                .Append(this.Headers).Append(Environment.NewLine)
                .Append(Environment.NewLine);

            return sb.ToString();
        }
    }
}
