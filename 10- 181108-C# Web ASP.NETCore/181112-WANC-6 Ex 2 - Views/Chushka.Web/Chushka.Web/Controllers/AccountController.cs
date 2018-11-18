using Chushka.Models;
using Chushka.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Chushka.Web.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<ChushkaUser> signInManager;

        public AccountController(SignInManager<ChushkaUser> signInManager)
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
            if(ModelState.IsValid)
            {
                var user = new ChushkaUser {
                    UserName = model.Username,
                    FullName = model.FullName,
                    Email = model.Email
                };

                var result = this.signInManager.UserManager.CreateAsync(user, model.Password).Result;
                
                if (result.Succeeded)
                {
                    if (this.signInManager.UserManager.Users.Count() == 1)
                    {
                        var roleResult = this.signInManager.UserManager.AddToRoleAsync(user, "Admin").Result;

                        if (roleResult.Errors.Any())
                        {
                            return this.View();
                        }
                    }
                    else
                    {
                        var roleResult = this.signInManager.UserManager.AddToRoleAsync(user, "User").Result;

                        if (roleResult.Errors.Any())
                        {
                            return this.View();
                        }
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
            if(ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

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
