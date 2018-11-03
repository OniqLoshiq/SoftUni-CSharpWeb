using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;
using SIS.Framework.Security;
using System.Collections.Generic;
using TORSHIA.App.Models.Binding;
using TORSHIA.App.Services.Contracts;
using TORSHIA.Models;

namespace TORSHIA.App.Controllers
{
    public class UsersController : BaseController
    {
        private IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginBindingModel model)
        {
            if (this.ModelState.IsValid != true)
            {
                return this.View();
            }

            User userFromDb = this.userService.GetUserByUsernameAndPassword(model);

            if (userFromDb == null)
            {
                return this.View();
            }

            IdentityUser user = new IdentityUser
            {
                Username = userFromDb.Username,
                Email = userFromDb.Email,
                IsValid = true,
                Roles = new List<string> { userFromDb.Role.ToString() }
            };

            this.SignIn(user);

            return this.RedirectToAction("/Home/Index");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterBindingModel model)
        {
            if(ModelState.IsValid != true && 
                ((model.Password != model.ConfirmPassword) && this.userService.HasUsername(model.Username)))
            {
                return this.View();
            }

            this.userService.CreateUser(model);

            return this.RedirectToAction("/Users/Login");
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.SignOut();

            return this.RedirectToAction("/Home/Index");
        }
    }
}
