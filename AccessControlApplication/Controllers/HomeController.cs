using AccessControlApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AccessControlApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static bool noUser = true;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            CombinedClasses obj = new();
            LoggedUser user = new();
            if (noUser)
            {
                noUser = false;
                user.CurrentUser = 0;
            }

            obj.User = user;

            return View(obj);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Register()
        {
            return RedirectToAction("Register", "Access");
        }

        public IActionResult LogIn()
        {
            return RedirectToAction("LogIn", "Access");
        }
        [HttpPost]
        public IActionResult Index(CombinedClasses obj)
        {
            return View(obj);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}