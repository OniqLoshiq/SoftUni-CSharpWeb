using SIS.WebServer;
using System;
using System.Reflection;

namespace SIS.MVCFramework
{
    public static class MvcEngine
    {
        private const string Default_ControllersSuffix = "Controller";
        private const string Default_ControllersFolder = "Controllers";
        private const string Default_ViewsFolder = "Views";
        private const string Default_ModelsFolder = "Models";
        private const string Default_RootDirRelativePath = "../../../";
        private const string Default_ResourceFolderName = "Resources";

        public static void Run(Server server)
        {
            RegisterAssemblyName();
            RegisterControllersData();
            RegisterViewsData();
            RegisterModelsData();
            RegisterResourceData();
            RegisterRootRelativePath();

            try
            {
                server.Run();
            }
            catch (Exception e)
            {
                //Log errors
                Console.WriteLine(e.Message);
            }
        }

        private static void RegisterRootRelativePath()
        {
            MvcContext.Get.RootDirectoryRelativePath = Default_RootDirRelativePath;
        }

        private static void RegisterResourceData()
        {
            MvcContext.Get.ResourceFolderName = Default_ResourceFolderName;
        }

        private static void RegisterModelsData()
        {
            MvcContext.Get.ModelsFolder = Default_ControllersFolder;
        }

        private static void RegisterViewsData()
        {
            MvcContext.Get.ViewsFolder = Default_ViewsFolder;
        }

        private static void RegisterControllersData()
        {
            MvcContext.Get.ControllersFolder = Default_ControllersFolder;
            MvcContext.Get.ControllersSuffix = Default_ControllersSuffix;
        }

        private static void RegisterAssemblyName()
        {
            MvcContext.Get.AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}
