namespace SIS.MVCFramework.Utilities
{
    public static class ControllerUtilities
    {
        public static string GetContollerName(object controller) => 
            controller.GetType().Name.Replace(MvcContext.Get.ControllersSuffix, string.Empty);

        public static string GetViewFullQualifiedName(string contollerName, string viewName) => 
            string.Format("../../../{0}/{1}/{2}.html", MvcContext.Get.ViewsFolder, contollerName, viewName);
    }
}
