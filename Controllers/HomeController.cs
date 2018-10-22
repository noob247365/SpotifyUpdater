using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spotify.Helpers;
using Spotify.Models;

namespace Spotify.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.TryGetValue("access_token", out string access_token))
                ViewData["access_token"] = access_token;
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
            return Redirect($"https://accounts.spotify.com/authorize?response_type=code&client_id={Credentials.ClientId}&redirect_uri={Uri.EscapeDataString(Credentials.RedirectUri)}");
        }

        public IActionResult Callback(string code = null, string error = null)
        {
            if (!string.IsNullOrWhiteSpace(error))
                return View("Error", new ErrorViewModel() { Message = $"Unable to authroize via Spotify: {error}"});
            Account.Login(this, code);
            return RedirectToAction("Index");
        }
    }
}
