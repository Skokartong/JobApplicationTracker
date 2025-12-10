using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;
public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> About()
    {
        return View();
    }
}