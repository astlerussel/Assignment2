using Assignment2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Assignment2.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["WelcomeMessage"] = GenerateWelcomeMessage();
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["WelcomeMessage"] = GenerateWelcomeMessage();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewData["WelcomeMessage"] = GenerateWelcomeMessage();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
