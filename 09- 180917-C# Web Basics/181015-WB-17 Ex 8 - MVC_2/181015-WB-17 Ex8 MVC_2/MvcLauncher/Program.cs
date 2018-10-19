using SIS.MVCFramework;
using SIS.MVCFramework.Routers;
using SIS.WebServer;

namespace MvcLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var handlingContext = new HttpRouteHandlingContext(new ControllerRouter(), new ResourceRouter());

            var server = new Server(8000, handlingContext);

            MvcEngine.Run(server);
        }
    }
}
