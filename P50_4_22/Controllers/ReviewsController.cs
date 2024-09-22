using Microsoft.AspNetCore.Mvc;

namespace P50_4_22.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
