namespace IdentityTutorial.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Mvc;

    public class HomeController : Controller
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly SignInManager<CustomUser> signInManager;

        public HomeController(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            ViewBag.HasVisitedAboutPage = false;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.HasVisitedAboutPage = User.HasClaim("about-page-visitor", "yes");
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> About()
        {
            if (!User.HasClaim(c => c.Type == "about-page-visitor"))
            {
                var user = await userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                await userManager.AddClaimAsync(user, new Claim("about-page-visitor", "yes"));

                await signInManager.RefreshSignInAsync(user);
            }

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
