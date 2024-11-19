using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThreadHaven.Models;

namespace ThreadHaven.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            // create a variable here and pass it to the view for display   
            ViewData["Timestamp"] = DateTime.Now;
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View("Privacy");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
