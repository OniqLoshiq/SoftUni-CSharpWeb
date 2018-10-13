using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using SIS_IRunes.Controllers;

namespace SIS_IRunes
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            GetServerRoutes(serverRoutingTable);

            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }

        private static void GetServerRoutes(ServerRoutingTable serverRoutingTable)
        {
            //User related routes
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new RedirectResult("/Home/Index");
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Home/Index"] = request => new HomeController().Index(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Login"] = request => new UsersController().LoginGet(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Login"] = request => new UsersController().LoginPost(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Register"] = request => new UsersController().RegisterGet(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Users/Register"] = request => new UsersController().RegisterPost(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Users/Logout"] = request => new UsersController().Logout(request);

            //Album related routes
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/All"] = request => new AlbumsController().All(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/Create"] = request => new AlbumsController().CreateGet(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Albums/Create"] = request => new AlbumsController().CreatePost(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Albums/Details"] = request => new AlbumsController().Details(request);

            //Track related routes
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Tracks/Create"] = request => new TracksController().CreateGet(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/Tracks/Create"] = request => new TracksController().CreatePost(request);

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/Tracks/Details"] = request => new TracksController().Details(request);
        }
    }
}
