using System;
using System.Net;

namespace _01_URLDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            string encodedUrl = string.Empty;

            while ((encodedUrl = Console.ReadLine()) != string.Empty)
            {
                string decodedUrl = WebUtility.UrlDecode(encodedUrl);

                Console.WriteLine(decodedUrl);
            }
        }
    }
}
