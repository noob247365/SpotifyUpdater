using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spotify.Models;

namespace Spotify.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Authorize()
        {
            // https://developer.spotify.com/documentation/general/guides/authorization-guide
            return Redirect($"https://accounts.spotify.com/authorize?response_type=code&client_id=8ad6eff46429447490ecd14a8b20b7b9&redirect_uri={Uri.EscapeDataString("https://localhost:5001/Home/Callback")}");
        }

        public IActionResult Callback(string code = null, string error = null)
        {
            if (!string.IsNullOrWhiteSpace(error))
                return View("Error", new ErrorViewModel() { Message = $"Unable to authroize via Spotify: {error}"});
            ViewData["Code"] = code;
            return View();
        }
    }
}
