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
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public RequestController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public IActionResult DetailsRequest(int requestId)
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
                                     where r.RequestId == requestId
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
