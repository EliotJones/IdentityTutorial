namespace IdentityTutorial.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Mvc;
    using ViewModels;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly SignInManager<CustomUser> signInManager;

        public AccountController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            return RedirectToActionPermanent("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new CustomUser(model.Email);

            var result = await userManager.CreateAsync(user, model.Password);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            signInManager.SignOut();
            return RedirectToAction("Index", "Home");
        } 
    }
}