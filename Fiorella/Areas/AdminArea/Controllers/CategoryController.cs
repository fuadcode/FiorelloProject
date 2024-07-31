using Fiorello.Areas.AdminArea.ViewModels.Category;
using Fiorello.Data;
using Fiorello.Models;
using Fiorello.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorella.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly FiorelloDbContext _context;

        public CategoryController(FiorelloDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var query = _context.Categories
                .AsNoTracking();
            return View(await PaginationVM<Category>.CreateVM(query, page, 2));

        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var details = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(k => k.Id == id);
            if (details is null) return BadRequest();
            return View(details);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid) return View(category);
            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Cant Create WIth Same Name");
                return View(category);
            }
            var newCategory = new Category()
            {
                Name = category.Name,
                CreatedDate = DateTime.Now

            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var details = await _context.Categories.FirstOrDefaultAsync(k => k.Id == id);
            if (details is null) return BadRequest();
            _context.Categories.Remove(details);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var details = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(k => k.Id == id);
            if (details is null) return BadRequest();



            return View(new CategoryUpdateVM {Name = details.Name });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM categoryUpdateVM)
        {
            if (id == null) return BadRequest();
            if (!ModelState.IsValid) return View(categoryUpdateVM);
            var details = await _context.Categories.FirstOrDefaultAsync(k => k.Id == id);
            if (details is null) return BadRequest();
            var existCategoryName = _context.Categories
                .Any(k => k.Name.ToLower() == categoryUpdateVM.Name.ToLower() && k.Id != id);
            if (existCategoryName)
            {
                ModelState.AddModelError("Name", "Given name already exists");
                return View(categoryUpdateVM);
            }
            details.Name = categoryUpdateVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
