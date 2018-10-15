namespace SIS.MVCFramework
{
    public class MvcContext
    {
        private const string Default_ControllersSuffix = "Controller";
        private const string Default_ControllersFolder = "Controllers";
        private const string Default_ViewsFolder = "Views";
        private const string Default_ModelsFolder = "Models";

        private static MvcContext Instance;

        private MvcContext() { }

        public static MvcContext Get => Instance ?? (Instance = new MvcContext());

        public string AssemblyName { get; set; }

        public string ControllersFolder { get; set; } = Default_ControllersFolder;

        public string ControllersSuffix { get; set; } = Default_ControllersSuffix;

        public string ViewsFolder { get; set; } = Default_ViewsFolder;

        public string ModelsFolder { get; set; } = Default_ModelsFolder;
    }
}
