using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OHD_SEM3.Models;
using OHD_SEM3.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OHD_SEM3.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace OHD_SEM3.Controllers
{
    [Authorize]
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AdminController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ListAccount()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<AccountViewModel>();
            foreach (User user in users)
            {
                var thisViewModel = new AccountViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(User user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> SetRole(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SetRole(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("ListAccount");
        }

        public IActionResult ListRequest()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<Request> _requests = db.Requests.ToList();
                List<Facility> _facilities = db.Facilities.ToList();
                List<IdentityUser> _users = db.Users.ToList();

                var listRequests = from r in _requests
                                   from f in _facilities
                                   where r.FacilityId == f.FacilityId
                                   from u in _users
                                   where r.requestorId == u.Id
                                   select new ViewModel1
                                   {
                                       _requests = r,
                                       _facilities = f,
                                       _users = u
                                   };
                return View(listRequests.ToList());

            }
        }

        public IActionResult DetailsRequest()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<Request> _requests = db.Requests.ToList();
                List<Facility> _facilities = db.Facilities.ToList();
                List<IdentityUser> _users = db.Users.ToList();

                var detailsRequest = from r in _requests
                                     from f in _facilities
                                     where r.FacilityId == f.FacilityId
                                     from u in _users
                                     where r.requestorId == u.Id
                                     where r.RequestId == 6
                                     select new ViewModel1
                                     {
                                         _requests = r,
                                         _facilities = f,
                                         _users = u
                                     };
                return View(detailsRequest);
            }
        }
        public IActionResult ApproveForm()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IEnumerable<IdentityUser> _users = db.Users.ToList();
                IEnumerable<IdentityRole> _roles = db.Roles.ToList();
                IEnumerable<IdentityUserRole<string>> _userrole = db.UserRoles.ToList();

                var listAssignee = from ur in _userrole
                                   from r in _roles
                                   where ur.RoleId == r.Id
                                   from u in _users
                                   where ur.UserId == u.Id
                                   where r.Name == "Assignee"
                                   select new ViewModel1
                                   {
                                       _roles = r,
                                       _users = u,
                                       _userrole = ur
                                   };
                return View(listAssignee);
            }
        }
        public async Task<IActionResult> SearchAccountByEmail(string searchString)
        {
            var users = await _userManager.Users.Where(s => s.Email.Contains(searchString)).ToListAsync();
            var userRolesViewModel = new List<AccountViewModel>();
            foreach (User user in users)
            {
                var thisViewModel = new AccountViewModel();
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName; 
                thisViewModel.UserName = user.UserName;
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
    }
}
