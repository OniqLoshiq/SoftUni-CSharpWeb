namespace SIS.HTTP.Extensions
{
    public static class HttpResponseStatusExtentions
    {
        public static string GetResponseLine(int responseCode)
        {
            string responseLine = string.Empty;

            switch (responseCode)
            {
                case 200: responseLine = "200 OK"; break;
                case 201: responseLine = "201 Created"; break;
                case 302: responseLine = "302 Found"; break;
                case 303: responseLine = "303 See Other"; break;
                case 400: responseLine = "400 Bad Request"; break;
                case 401: responseLine = "401 Unauthorized"; break;
                case 403: responseLine = "403 Forbidden"; break;
                case 404: responseLine = "404 Not Found"; break;

                default: responseLine = "500 Internal Server Error"; break;
            }

            return responseLine;
        }
    }
}
