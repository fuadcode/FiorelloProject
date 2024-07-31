using Fiorello.Data;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        readonly FiorelloDbContext _context;

        public ProductViewComponent(FiorelloDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int take = 4)
        {
            var products = _context.Products.Take(take).ToList();
            return View(await Task.FromResult(products));
        }
    }
}
