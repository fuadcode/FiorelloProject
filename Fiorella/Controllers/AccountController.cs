using Fiorello.Helpers;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Fiorello.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Fiorello.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            AppUser user = new()
            {
                UserName = registerVM.UserName,
                FullName = registerVM.FullName,
                Email = registerVM.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string link = Url.Action(nameof(VerifyEmail), "Account", new { email = user.Email, token },
                Request.Scheme, Request.Host.ToString());

            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/templates/emailTemplate/emailConfirm.html"))
            {
                body = reader.ReadToEnd();
            };
            body = body.Replace("{{link}}", link);
            body = body.Replace("{{username}}", user.FullName);
            _emailService.SendEmail(new() { user.Email }, body, "Email verification", "Verify email");


            await _userManager.AddToRoleAsync(user, nameof(RolesEnum.Member));
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null) return NotFound();
            await _userManager.ConfirmEmailAsync(appUser, token);
            await _signInManager.SignInAsync(appUser, true);
            return RedirectToAction("index", "home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            var user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username or Email is wrong...");
                    return View(loginVM);

                }
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "User is Lockout.. ");
                return View(loginVM);
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Go to verify EmailAdress.. ");
                return View(loginVM);
            }
            if (user.IsBlocked)
            {
                ModelState.AddModelError("", "User is Blocked.. ");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Email is wrong ");
                return View(loginVM);
            }
            return RedirectToAction("index", "home");

        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

       

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Email is required");
                return View();
            }

            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", "Given email does not exist");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string url = Url.Action(nameof(ResetPassword), "Account"
                , new { email = user.Email, token = token }
                , Request.Scheme
                , Request.Host.ToString());

            string body;
            using (StreamReader reader = new StreamReader("wwwroot/templates/forgetpasswordTemplate/forgotpassword.html"))
            {
                body = await reader.ReadToEndAsync();
            }
            body = body.Replace("{{link}}", url);
            body = body.Replace("{{username}}", user.UserName);

            _emailService.SendEmail(new() { user.Email }, body, "Forget password", "Reset password");

            return RedirectToAction("index", "home");
        }

        private string GetHtmlTemplate(string filePath)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
            using (StreamReader reader = new StreamReader(fullPath))
            {
                return reader.ReadToEnd();
            }
        }

        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            var existUser = await _userManager.FindByEmailAsync(email);
            if (existUser == null) return NotFound();

            bool result = await _userManager.VerifyUserTokenAsync(existUser, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);

            if (result == false)
            {
                string templateContent = GetHtmlTemplate("templates/resetpasswordmessageTemplate/resetpasswordmessage.html");
                return Content(templateContent, "text/html");
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email, string token, ResetPasswordVM request)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);

            if (!ModelState.IsValid) return View();

            await _userManager.ResetPasswordAsync(appUser, token, request.Password);
            await _userManager.UpdateSecurityStampAsync(appUser);
            await _signInManager.SignInAsync(appUser, true);

            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> AddRole()
        {
            if (!await _roleManager.RoleExistsAsync("admin"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "admin" });
            if (!await _roleManager.RoleExistsAsync("member"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "member" });
            if (!await _roleManager.RoleExistsAsync("superadmin"))
                await _roleManager.CreateAsync(new IdentityRole { Name = "superadmin" });
            return Content("Roles added");

        }
    }
}