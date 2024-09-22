using Microsoft.AspNetCore.Mvc;

namespace P50_4_22.Controllers
{
    public class Catalog : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
