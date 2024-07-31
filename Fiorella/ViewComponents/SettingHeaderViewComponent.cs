using Fiorello.Data;
using Fiorello.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.ViewComponents
{
    public class SettingHeaderViewComponent : ViewComponent
    {
        private readonly FiorelloDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SettingHeaderViewComponent(FiorelloDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.FullName = user.FullName;
            }
            var setting = _context.Settings.ToDictionary(k => k.Key, v => v.Value);
            return View(await Task.FromResult(setting));
        }
    }
}
