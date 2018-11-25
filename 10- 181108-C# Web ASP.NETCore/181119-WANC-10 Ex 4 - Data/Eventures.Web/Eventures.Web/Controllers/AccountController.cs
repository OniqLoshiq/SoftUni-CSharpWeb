using Eventures.Common;
using Eventures.Data;
using Eventures.Models;
using Eventures.Web.Filters;
using Eventures.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.Web.Controllers
{
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    public class AccountController : Controller
    {
        private SignInManager<EventuresUser> signInManager;

        private EventuresDbContext db;

        public AccountController(SignInManager<EventuresUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new EventuresUser
                {
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UCN = model.UCN
                };

                var result = this.signInManager.UserManager.CreateAsync(user, model.Password).Result;

                if (result.Succeeded)
                {
                    var roleResult = this.signInManager.UserManager.AddToRoleAsync(user, "User").Result;

                    if (roleResult.Errors.Any())
                    {
                        return this.View();
                    }

                    await this.signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
            }

            return this.View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, ErrorMessages.InvalidLogin);

            return this.View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
