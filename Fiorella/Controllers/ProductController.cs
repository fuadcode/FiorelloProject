using Fiorello.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Controllers
{
    public class ProductController : Controller
    {
        readonly FiorelloDbContext _context;

        public ProductController(FiorelloDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var datas = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .ToList();
            ViewBag.ProductCount = datas.Count;

            return View(datas);
        }
        public IActionResult Detail(int? id)
        {
            if (id is null) return BadRequest();
            var data = _context.Products.Include(p => p.ProductImages).FirstOrDefault(n => n.Id == id);
            if (data is null) return BadRequest();
            return View(data);
        }
        public IActionResult LoadMore(int offset = 4)
        {
            var products = _context.Products.Include(m => m.ProductImages).Skip(offset).Take(4).ToList();
            return PartialView("_ProductPartialView", products);
        }
        public IActionResult SearchProduct(string text)
        {
            var datas = _context.Products.Where(n => n.Name.ToLower().Contains(text.ToLower())).Take(5).ToList();
            return PartialView("_ProductSearchPartial", datas);
        }
    }
}
