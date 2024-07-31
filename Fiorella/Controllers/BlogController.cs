using Fiorello.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Controllers
{
    public class BlogController : Controller
    {
        readonly FiorelloDbContext _context;

        public BlogController(FiorelloDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.BlogCount = _context.Blogs.Count();
            return View();
        }
        public IActionResult Detail(int? id)
        {
            if (id is null) return BadRequest();
            var blog = _context.Blogs.AsNoTracking().FirstOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }
        public IActionResult LoadMore(int offset = 3)
        {
            var datas = _context.Blogs.Skip(offset).Take(3).ToList();
            return PartialView("_BlogPartialView", datas);
        }
        public IActionResult SearchBlog(string text)
        {
            var datas = _context.Blogs.Where(n => n.Title.ToLower().Contains(text.ToLower())).Take(5).ToList();
            return PartialView("_SearchPartialView", datas);
        }
    }
}
