using Fiorello.Data;
using Fiorello.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fiorello.Controllers
{
    public class BasketController : Controller
    {
        readonly FiorelloDbContext _context;

        public BasketController(FiorelloDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddBasket(int? id)
        {
            if (id is null) return BadRequest();
            var existProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existProduct is null) return BadRequest();
            string basket = Request.Cookies["basket"];
            List<BasketVM> list;
            if (basket is null)
            {
                list = new();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            var existProductBasket = list.FirstOrDefault(l => l.Id == id);
            if (existProductBasket is null)
                list.Add(new BasketVM() { Id = existProduct.Id, BasketCount = 1 });
            else
                existProductBasket.BasketCount++;
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(list));
            return RedirectToAction("Index", "Home");
        }


        public IActionResult ShowBasket()
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> list;
            if (basket is null)
            {
                list = new();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var basketProduct in list)
                {
                    var existProduct = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == basketProduct.Id);
                    basketProduct.Name = existProduct.Name;
                    basketProduct.ImageUrl = existProduct.ProductImages.FirstOrDefault(i => i.IsMain).ImageUrl;
                }
            }
            return View(list);
        }
        public IActionResult Delete(int? id)
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            var deletedItem = list.FirstOrDefault(d => d.Id == id);
            if (deletedItem != null)
            {
                list.Remove(deletedItem);
                Response.Cookies.Append("basket", JsonConvert.SerializeObject(list));
            }
            return RedirectToAction("ShowBasket");
        }

    }
}
