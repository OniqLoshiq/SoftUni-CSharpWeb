using SIS.MVCFramework.ActionResults.Contracts;
using SIS.MVCFramework.Controllers;

namespace MvcLauncher.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
