using Microsoft.AspNetCore.Mvc;

namespace P50_4_22.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
