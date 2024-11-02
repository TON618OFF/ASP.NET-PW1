using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;

namespace P50_4_22.Controllers
{
    public class CatalogController : Controller
    {
        public BulkinKeysContext db;

        public CatalogController(BulkinKeysContext context)
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
        
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Product product)
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(Product => Product.IdProduct == id);
                if(product != null)
                {
                    return View(product);
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.IdProduct == id);
                if (product != null)
                {
                    return View(product);
                }
            }
            return NotFound();
        }
        [HttpPost]

        public async Task<IActionResult> Edit(Product product)
        {
            db.Products.Update(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]

        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.IdProduct == id);
                if( product != null)
                {
                    return View(product);
                }
            }
            return NotFound();
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int? id)
        {
            if(id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.IdProduct == id);
                if(product != null)
                {
                    db.Products.Remove(product);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
