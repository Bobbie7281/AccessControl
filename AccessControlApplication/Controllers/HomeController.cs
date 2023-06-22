using AccessControlApplication.Data;
using AccessControlApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AccessControlApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //[HttpPost]
        public IActionResult Register()
        {
            return RedirectToAction("Register", "Access");
        }
        public IActionResult LogIn()
        {
            return RedirectToAction("LogIn", "Access");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}