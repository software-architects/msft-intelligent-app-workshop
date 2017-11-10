using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAppWithAI.Models;
using System.Net.Http;

namespace WebAppWithAI.Controllers
{
    public class HomeController : Controller
    {
        private static int delay = 200;

        public async Task<IActionResult> Index()
        {
            await Task.Delay(delay);

            delay = delay * 2;

            return View();
        }

        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";

            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync("http://request-to-nothing.com");
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
