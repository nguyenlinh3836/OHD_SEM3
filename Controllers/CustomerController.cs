using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OHD_SEM3.Data;
using OHD_SEM3.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;

namespace OHD_SEM3.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private UserManager<User> _userManager;
        private ApplicationDbContext _context;
        public CustomerController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Welcome()
        {
            return View();
        }
        public IActionResult Done()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Facilities.ToListAsync());
        }
        [TempData] 
        public string StatusMessage { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRequest(Request request)
        {         
        if (ModelState.IsValid)
            {
                var User = await _userManager.GetUserAsync(HttpContext.User);              
                request.CreateTime = DateTime.Now;
                request.requestorId = User.Id;
                request.Status = "Pending";
                _context.Add(request);
                await _context.SaveChangesAsync();             
                return RedirectToAction(nameof(Done));
            }
            return View(request);
        }
        public async Task<IActionResult> History()
        {
            var User = await _userManager.GetUserAsync(HttpContext.User);
            var ListRequest = await _context.Requests.Where(x => x.requestorId == User.Id).ToListAsync();
            return View(ListRequest);
        }

    }
}
