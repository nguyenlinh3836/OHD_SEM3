using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using OHD_SEM3.Data;
using OHD_SEM3.Models;

namespace OHD_SEM3.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private ApplicationDbContext _context;
        private RoleManager<IdentityRole> _roleManager;

        public LoginModel(SignInManager<User> signInManager,
            ILogger<LoginModel> logger,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            await _roleManager.CreateAsync(new IdentityRole("Assignee"));
            await _roleManager.CreateAsync(new IdentityRole("Customer"));
            var userAdmin = new User();
            userAdmin.UserName = "Admin@admin.com";
            userAdmin.Email = "Admin@admin.com";
            string userPwd = "Admin123";
            IdentityResult chkUser = await _userManager.CreateAsync(userAdmin, userPwd);
            if (chkUser.Succeeded)
            {
                var result = await _userManager.AddToRoleAsync(userAdmin, "Administrator");
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    var User = await _userManager.FindByEmailAsync(Input.Email);
                    var roleName = await _userManager.GetRolesAsync(User);
                    HttpContext.Session.SetString("role",roleName.FirstOrDefault());
                   
                    if (roleName.FirstOrDefault() == "Administrator")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    if (roleName.FirstOrDefault() == "Assignee")
                    {
                        return RedirectToAction("Index", "Assignee");
                    }
                    if (roleName.FirstOrDefault() == "Customer")
                    {
                        return RedirectToAction("Index", "Customer");
                    }
                    if (roleName.FirstOrDefault() == "User")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}