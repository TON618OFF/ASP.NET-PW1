using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace P50_4_22.Controllers
{
    public class HomeController : Controller
    {
        public BulkinKeysContext db;

        public HomeController(BulkinKeysContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            //var product = db.Products.ToList();
            //var user = db.Clients.Include(u => u.IdClient).ToList();
            //var pu = new ProductUserVM
            //{
            //    Products = product,
            //    Users = user
            //};
            return View(await db.Products.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
