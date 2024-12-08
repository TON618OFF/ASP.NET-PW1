using Microsoft.AspNetCore.Mvc;
using P50_4_22.Models;

namespace P50_4_22.Controllers
{
    public class SearchController : Controller
    {
        private readonly BulkinKeysContext _context; // Контекст базы данных

        public SearchController(BulkinKeysContext context)
        {
            _context = context;
        }

        // Метод для отображения страницы поиска
        [HttpGet]
        public IActionResult Index(string searchQuery)
        {
            ViewData["SearchQuery"] = searchQuery; // Передаем текущий запрос в представление

            // Если запрос пустой, возвращаем все товары или ничего, по вашему усмотрению
            var products = string.IsNullOrEmpty(searchQuery)
                ? new List<P50_4_22.Models.Product>()
                : _context.Products
                    .Where(p => p.ProductName.Contains(searchQuery)) // Фильтруем по названию
                    .Select(p => new P50_4_22.Models.Product
                    {
                        IdProduct = p.IdProduct,
                        ProductName = p.ProductName,
                        ProductDescription = p.ProductDescription,
                        ProductPrice = p.ProductPrice,
                        ProductImage = p.ProductImage // Убедитесь, что поле существует
                    })
                    .ToList();

            return View(products);
        }

        // Метод для отображения подробной информации о товаре
        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Where(p => p.IdProduct == id)
                .Select(p => new P50_4_22.Models.Product
                {
                    IdProduct = p.IdProduct,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductPrice = p.ProductPrice,
                    ProductImage = p.ProductImage // Убедитесь, что поле существует
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            return View(product); // Создайте представление Details.cshtml для отображения информации
        }

    }
}
