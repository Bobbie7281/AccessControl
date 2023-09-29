using AccessControlApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AccessControlApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController>? _logger;
        readonly ICombinedClasses _combinedClasses;
        readonly ILoggedUser _loggedUser;
        public HomeController(ILogger<HomeController> logger, ICombinedClasses combinedClasses, ILoggedUser loggedUser)
        {
            _logger = logger;
            _combinedClasses = combinedClasses;
            _loggedUser = loggedUser;
        }


        public IActionResult Index()
        {
            _combinedClasses.User = (LoggedUser)_loggedUser;

            if (!_loggedUser.UserCheck)
            {
                _loggedUser.CurrentUser = 0;
                _loggedUser.AdminRights = false;
            }
            return View(_combinedClasses);
        }

        public IActionResult Privacy()
        {
            _combinedClasses.User = (LoggedUser)_loggedUser;

            return View(_combinedClasses);
        }
        public IActionResult Register()
        {
            return RedirectToAction("Register", "Access");
        }

        public IActionResult DisplayUsers() 
        {
            return RedirectToAction("DisplayUsers", "Access");
        }

        public IActionResult LogIn()
        {
            return RedirectToAction("LogIn", "Access");
        }
        public IActionResult LogOff()
        {
            _loggedUser.UserCheck = false;
            return RedirectToAction("Index", "Home");
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