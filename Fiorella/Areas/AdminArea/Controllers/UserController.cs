
using Fiorello.Areas.AdminArea.ViewModels.UserVms;
using Fiorello.Helpers;
using Fiorello.Models;
using Fiorello.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Fiorello.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;

        }
     
        public async Task<IActionResult> Index(string searchText, int stage = 1)
        {
            var users = string.IsNullOrEmpty(searchText) ? await _userManager.Users.ToListAsync()
                   : await _userManager.Users.Where(u => u.UserName.ToLower().Contains(searchText.ToLower()) ||
                u.FullName.ToLower().Contains(searchText.ToLower())).ToListAsync();

            return View(users);
           

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateUserVM createUserVM)
        {
            if (!ModelState.IsValid) return View(createUserVM);
            AppUser user = new()
            {
                FullName = createUserVM.FullName,
                UserName = createUserVM.UserName,
                Email = createUserVM.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(user, createUserVM.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(createUserVM);

        }

        await _userManager.AddToRoleAsync(user, nameof(RolesEnum.Member));


        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string link = Url.Action(nameof(VerifyEmail), "Account", new { email = user.Email, token },
            Request.Scheme, Request.Host.ToString());

        string body = string.Empty;
        using (StreamReader reader = new StreamReader("wwwroot/templates/emailTemplate/emailConfirm.html"))
        {
            body = reader.ReadToEnd();
        };
        body = body.Replace("{{link}}", link);
        body = body.Replace("{{username}}", user.UserName);
        _emailService.SendEmail(new() { user.Email }, body, "Email verification", "Verify email");

        await _userManager.AddToRoleAsync(user, RolesEnum.Member.ToString());
        return RedirectToAction("index", "user");
    }


    public async Task<IActionResult> ChangeStatus(string id)
        {
            if (id is null) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest();

            user.IsBlocked = !user.IsBlocked;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("index");
        }



        public async Task<IActionResult> Detail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new UserListVM
                {
                    FullName = m.FullName,
                    UserName = m.UserName,
                    Email = m.Email,
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id is null) return BadRequest();
            var role = await _userManager.FindByIdAsync(id);
            if (role is null) return NotFound();
            await _userManager.DeleteAsync(role);
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserUpdateVM
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FullName = model.FullName;
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index"); 
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

             public async Task<IActionResult> VerifyEmail(string email, string token)
{
             AppUser user = await _userManager.FindByEmailAsync(email);
             if (user is null) return BadRequest();
             await _userManager.ConfirmEmailAsync(user, token);
             await _signInManager.SignInAsync(user, true);
             return RedirectToAction("index", "home");
}
        
    }
}