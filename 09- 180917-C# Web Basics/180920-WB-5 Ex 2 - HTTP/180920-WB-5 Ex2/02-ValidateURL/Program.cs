using System;
using System.Net;
using System.Text;

namespace _02_ValidateURL
{
    class Program
    {
        static string ERROR_MSG = "Invalid URL";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Enter your ulr input (to end the programm just press 'enter'): ");
                string encodedUrl = Console.ReadLine();

                var sb = new StringBuilder();

                if(encodedUrl == string.Empty)
                {
                    break;
                }

                string decodedUrl = WebUtility.UrlDecode(encodedUrl);

                try
                {
                    var uri = new Uri(decodedUrl);

                    if (!((uri.Scheme == "http" && uri.IsDefaultPort) ^ (uri.Scheme == "https" && uri.IsDefaultPort)))
                    {
                        Console.WriteLine(ERROR_MSG);
                        continue;
                    }

                    sb.AppendLine($"Protocol: {uri.Scheme}")
                        .AppendLine($"Host: {uri.Host}")
                        .AppendLine($"Port: {uri.Port}")
                        .AppendLine($"Path: {uri.AbsolutePath}");

                    if(!string.IsNullOrEmpty(uri.Query))
                    {
                        sb.AppendLine($"Query: {uri.Query.TrimStart('?')}");
                    }

                    if(!string.IsNullOrEmpty(uri.Fragment))
                    {
                        sb.AppendLine($"Fragment: {uri.Fragment}");
                    }

                    Console.WriteLine(sb.ToString().Trim());

                }
                catch (Exception)
                {
                    Console.WriteLine(ERROR_MSG);
                }
            }
        }
    }
}
