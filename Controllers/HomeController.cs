using Dziennik_elektroniczny.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dziennik_elektroniczny.Controllers
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
            _logger.LogInformation("Wy�wietlono stron� g��wn�.");
            return View();
        }

        public IActionResult About()
        {
            _logger.LogInformation("Wy�wietlono stron� 'O nas'.");
            return View();
        }

   
        [Authorize]
        public IActionResult Dashboard()
        {
            _logger.LogInformation("U�ytkownik zalogowany wy�wietli� Dashboard.");
            return View();
        }


        [Authorize(Roles = "Administrator")]
        public IActionResult AdminPanel()
        {
            _logger.LogInformation("Administrator uzyska� dost�p do panelu administracyjnego.");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Wyst�pi� b��d w aplikacji.");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
