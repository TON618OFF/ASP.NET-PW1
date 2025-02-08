using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using P50_4_22.Models;

namespace P50_4_22.ViewComponents
{
    [ViewComponent] // Добавьте этот атрибут, если хотите явное указание компонента
    public class AverageRatingViewComponent : ViewComponent
    {
        private readonly BulkinKeysContext _context;

        public AverageRatingViewComponent(BulkinKeysContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            double averageRating = reviews.Any() ? reviews.Average(r => r.ReviewRating) : 0;

            Console.WriteLine($"Средний рейтинг для продукта {productId}: {averageRating}");

            return View("Default", averageRating);
        }
    }
}
