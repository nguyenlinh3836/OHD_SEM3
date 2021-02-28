using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OHD_SEM3.Models;
using OHD_SEM3.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Http;

namespace OHD_SEM3.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var _role = HttpContext.Session.GetString("role");
            if (_role == "Administrator")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (_role == "Assignee")
            {
                return RedirectToAction("Index", "Assignee");
            }
            else if (_role == "Customer")
            {
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                return View();
            }
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
    }
}
