using Microsoft.AspNetCore.Mvc;
using Fiorello.Data;


namespace EduHome.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class DashBoardController : Controller
    {
        private readonly FiorelloDbContext _context;

        public DashBoardController(FiorelloDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userCount = _context.Users.Count();
            var subscriptionCount = _context.Subscriptions.Count();

            ViewBag.UserCount = userCount;
            ViewBag.SubscriptionCount = subscriptionCount;

            return View();
        }
    }
}

