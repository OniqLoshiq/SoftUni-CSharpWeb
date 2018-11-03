using SIS.Framework;

namespace TORSHIA.App
{
    class Launcher
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());
        }
    }
}
