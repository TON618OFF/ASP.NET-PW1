using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;
using System.Security.Claims;

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

        [Route("ProductDetails/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Reviews) // Подгружаем отзывы
                    .ThenInclude(r => r.ClientName) // Подгружаем пользователей для каждого отзыва
                .FirstOrDefaultAsync(p => p.IdProduct == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
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


        [HttpPost]
        [Route("Catalog/AddReview")]
        public async Task<IActionResult> AddReview(Review review)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Authorize/Authorize"); // Если пользователь не авторизован
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Не удалось определить пользователя.");
            }

            // Принудительная конвертация строки в число, если не сработает - устанавливаем 0 (что вызовет ошибку)
            if (!int.TryParse(Request.Form["ReviewRating"], out int rating))
            {
                rating = 0;
            }
            review.ReviewRating = rating;

            if (review.ReviewRating < 1 || review.ReviewRating > 5)
            {
                ModelState.AddModelError("ReviewRating", "Оценка должна быть от 1 до 5.");
                return View("Details");
            }

            if (string.IsNullOrWhiteSpace(review.ReviewComment))
            {
                ModelState.AddModelError("ReviewComment", "Комментарий не может быть пустым.");
                return View("Details");
            }

            review.ClientNameId = userId;
            review.ReviewCreatedDate = DateTime.Now;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = review.ProductId });
        }


    }
}
