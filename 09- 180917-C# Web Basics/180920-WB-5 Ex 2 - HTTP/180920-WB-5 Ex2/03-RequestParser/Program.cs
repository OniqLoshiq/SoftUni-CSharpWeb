using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace _03_RequestParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpRequests = new Dictionary<string, HashSet<string>>(); //method, path

            string input = string.Empty;

            while((input = Console.ReadLine().ToLower()) != "end")
            {
                string[] inputData = input.Split('/', StringSplitOptions.RemoveEmptyEntries);

                string requestMethod = inputData[1];
                string requestPath = inputData[0];

                if(!httpRequests.ContainsKey(requestMethod))
                {
                    httpRequests[requestMethod] = new HashSet<string>();
                }

                httpRequests[requestMethod].Add(requestPath);
            }

            string[] httpRespondData = Console.ReadLine().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string respondMethod = httpRespondData[0];
            string respondPath = httpRespondData[1].Substring(1);
            string httpVersion = httpRespondData[2].ToUpper();

            string output = string.Empty;

            if(httpRequests.ContainsKey(respondMethod))
            {
                if(httpRequests[respondMethod].Contains(respondPath))
                {
                    output = GetRespondOutput(httpVersion, (int)HttpStatusCode.OK, HttpStatusCode.OK.ToString());
                }
            }

            string respond = string.IsNullOrEmpty(output) ? GetRespondOutput(httpVersion, (int)HttpStatusCode.NotFound, HttpStatusCode.NotFound.ToString()) : output;
            
            Console.WriteLine(respond);
        }

        private static string GetRespondOutput(string httpVersion, int statusCode, string statusText)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{httpVersion} {statusCode} {statusText}")
                .AppendLine($"Content-Length: {statusText.Length}")
                .AppendLine("Content-Type: text/plain")
                .AppendLine()
                .AppendLine(statusText);

            return sb.ToString().Trim();
        }
    }
}
