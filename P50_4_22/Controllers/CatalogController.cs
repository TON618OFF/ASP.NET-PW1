using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;

namespace P50_4_22.Controllers
{
    public class CatalogController : Controller
    {
        public BulkinKeysContext _context;

        public CatalogController(BulkinKeysContext context)
        {
            _context = context;
        }

        public IActionResult Index(string category, string priceRange, string sortOrder)
        {
            // Получение всех продуктов
            var products = _context.Products.AsQueryable();

            // Фильтрация по категории
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.CategoryName.ToLower() == category.ToLower());
            }

            // Фильтрация по диапазону цен
            if (!string.IsNullOrEmpty(priceRange))
            {
                var priceParts = priceRange.Split('-');
                if (priceParts.Length == 2 &&
                    decimal.TryParse(priceParts[0], out var minPrice) &&
                    decimal.TryParse(priceParts[1], out var maxPrice))
                {
                    products = products.Where(p => p.ProductPrice >= minPrice && p.ProductPrice <= maxPrice);
                }
            }

            // Сортировка
            products = sortOrder switch
            {
                "priceAsc" => products.OrderBy(p => p.ProductPrice),
                "priceDesc" => products.OrderByDescending(p => p.ProductPrice),
                "nameAsc" => products.OrderBy(p => p.ProductName),
                "nameDesc" => products.OrderByDescending(p => p.ProductName),
                _ => products // Без сортировки
            };

            // Передача списка в представление
            var model = products.ToList();
            return View(model);
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(Product => Product.IdProduct == id);
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
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.IdProduct == id);
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
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.IdProduct == id);
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
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.IdProduct == id);
                if(product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
