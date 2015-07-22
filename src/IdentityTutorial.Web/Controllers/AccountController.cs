namespace IdentityTutorial.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
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
        private readonly IUserStore<CustomUser> store;

        public AccountController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager, IUserStore<CustomUser> store)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.store = store;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel
            {
                ExternalProviders = signInManager.GetExternalAuthenticationSchemes().ToArray()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            model.ExternalProviders = signInManager.GetExternalAuthenticationSchemes().ToArray();
            if (!ModelState.IsValid)
            {
                model.Errors.AddModelStateErrors(ModelState);
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            
            model.Errors.Add("Sign-in failed. Please try again. If you cannot sign-in you might not be registered");

            return View(model);
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
                model.Errors.AddModelStateErrors(ModelState);
                return View(model);
            }

            var user = new CustomUser(model.Email);

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var signInResult = await signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                model.Errors.Add("Registration was successful however sign-in failed. Please try to sign in again");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    model.Errors.Add(error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            signInManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback");
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await signInManager.GetExternalLoginInfoAsync();

            // Sign in the user with this external login provider if the user already has a login
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // If the user does not have an account, then create an external only account
                var user = CustomUser.CreateFromExternalSource(new CustomLogin
                {
                    LoginProvider = info.LoginProvider,
                    ProviderDisplayName = info.LoginProvider,
                    ProviderKey = info.ProviderKey
                }, info.ExternalPrincipal.GetUserName());

                await userManager.CreateAsync(user);

                result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Login");
            }
        }
    }
}