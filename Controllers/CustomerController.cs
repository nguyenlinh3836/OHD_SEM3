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

namespace OHD_SEM3.Controllers
{
    public class CustomerController : Controller
    {
        private UserManager<User> UserManager;
        private ApplicationDbContext _context;
        public CustomerController(UserManager<User> userManager, ApplicationDbContext context)
        {
            UserManager = userManager;
            _context = context;
        }

        public IActionResult Welcome()
        {
            return View();
        }
       
        public async Task<IActionResult> Index()
        {
            return View(await _context.Facilities.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRequest(Request request)
        {
            User applicationUser = await UserManager.GetUserAsync(User);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// will give the user's userId       

          
            if (ModelState.IsValid)
            {
                request.CreateTime = DateTime.Now;
                request.requestorId = userId;
                _context.Add(Request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }
        
    }
}
