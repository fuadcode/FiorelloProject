using Fiorello.Data;
using Fiorello.Services.Interfaces;
using Fiorello.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fiorello.Services
{
    public class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly FiorelloDbContext _context;

        public BasketService(FiorelloDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public int GetBasketCount() => GetBasketCountFromCookies().Count();


        public List<BasketVM> GetBasketList()
        {
            var list = GetBasketCountFromCookies();
            foreach (var basketProduct in list)
            {
                var existProduct = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p => p.Id == basketProduct.Id);
                basketProduct.Name = existProduct.Name;
                basketProduct.ImageUrl = existProduct.ProductImages.FirstOrDefault(i => i.IsMain).ImageUrl;
            }
            return list;
        }
        private List<BasketVM> GetBasketCountFromCookies()
        {
            List<BasketVM> list = new List<BasketVM>();
            string basket = _contextAccessor.HttpContext.Request.Cookies["basket"];
            if (basket != null)
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }

            return list;
        }
    }
}
