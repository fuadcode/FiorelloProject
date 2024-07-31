using Fiorello.Data;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.ViewComponents
{
    public class SettingFooterViewComponent : ViewComponent
    {
        readonly FiorelloDbContext _context;

        public SettingFooterViewComponent(FiorelloDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = _context.Settings.ToDictionary(k => k.Key, v => v.Value);
            return View(await Task.FromResult(setting));
        }
    }
}
