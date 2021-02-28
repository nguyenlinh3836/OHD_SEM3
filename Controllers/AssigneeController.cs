using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OHD_SEM3.Data;
using OHD_SEM3.Models;

namespace OHD_SEM3.Controllers
{
    public class AssigneeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AssigneeController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var User = await _userManager.GetUserAsync(HttpContext.User);
            var ListRequest = await _context.Requests.Where(x => x.AssigneeId == User.Id).ToListAsync();
            return View(ListRequest);
        }
        [HttpPost]
        public async Task<IActionResult> RejectRequest([Bind("Status,Remark")] Request request, int AssignId)
        {
            if (AssignId != request.RequestId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                request.UpdateTime = DateTime.Now;             
                _context.Update(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

    }
}
