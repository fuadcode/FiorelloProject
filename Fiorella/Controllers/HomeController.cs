using Fiorello.Data;
using Fiorello.Models;
using Fiorello.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Controllers
{
    public class HomeController : Controller
    {
        private readonly FiorelloDbContext _context;

        public HomeController(FiorelloDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {


            var sliders = _context.Sliders.AsNoTracking().ToList();
            var sliderContent = _context.SliderContent.AsNoTracking().SingleOrDefault();
            var categories = _context.Categories.ToList();
            var products = _context.Products.Include(p => p.ProductImages).ToList();
            var subscription = _context.Subscriptions.AsNoTracking().FirstOrDefault();
            var expert = _context.Experts.AsNoTracking().ToList();
            var homeVM = new HomeVM()
            {
                Sliders = sliders,
                SliderContent = sliderContent,
                Categories = categories,
                Products = products,
                Subscriptions = subscription,
                Experts= expert,
            };
            return View(homeVM);
        }

        [HttpPost]
        [Route("subscribe")]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                return Json(new { success = false, message = "Invalid email address!" });
            }

            var existingSubscription = await _context.Subscriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Email == email);

            if (existingSubscription != null)
            {
                return Json(new { success = false, message = "This email is already subscribed!" });
            }

            var subscription = new Subscription
            {
                SubscriptionName = "UserSubscription",
                Email = email
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Subscription Success!" });
        }
        private bool IsValidEmail(string email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }
    }
}



  
