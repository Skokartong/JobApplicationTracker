using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
