using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl = "/Job/Index")
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = returnUrl
            };

            return Challenge(authenticationProperties, "Auth0");
        }

        public IActionResult Logout()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = "/"
            };

            return SignOut(authenticationProperties, "Auth0", CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
