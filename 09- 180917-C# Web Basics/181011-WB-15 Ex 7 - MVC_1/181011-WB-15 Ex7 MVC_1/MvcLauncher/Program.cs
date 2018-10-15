using SIS.MVCFramework;
using SIS.MVCFramework.Routers;
using SIS.WebServer;
using System;
using System.Reflection;

namespace MvcLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(8000, new ControllerRouter());

            MvcContext.Get.AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            try
            {
                server.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
