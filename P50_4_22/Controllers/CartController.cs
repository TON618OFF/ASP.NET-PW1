using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;

namespace P50_4_22.Controllers
{
    public class CartController : Controller
    {

        private readonly BulkinKeysContext _context;

        public CartController(BulkinKeysContext context)
        {
            _context = context;
        }

		public IActionResult Index()
		{
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int user))
			{
				return RedirectToAction("Index", "Authorize");
			}

			var userIntId = Convert.ToInt32(userId);
			var cartItems = _context.CartItems
				.Where(c => c.ClientId == userIntId)
				.Include(c => c.Product)
				.ToList();

			return View(cartItems);
		}




		[HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int amount)
        {
            var userId = User.Identity?.Name;
            if (userId == null)
            {
                return RedirectToAction("Index", "Authorize");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Продукт не найден!");
            }

            if (product.ProductAmount < amount)
            {
                return BadRequest("Недостаточно товара на складе");
            }

            int userIntId = Convert.ToInt32(userId);
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.ClientId == userIntId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = amount,
                    Price = product.ProductAmount * amount,
                    ClientId = userIntId
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += amount;

                if (cartItem.Quantity > product.ProductAmount)
                {
                    return BadRequest("Недостаточно товара на складе для обновлённого количества.");
                }

                cartItem.Price = cartItem.Quantity * product.ProductPrice;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            var userId = User.Identity?.Name;

            if (userId == null)
            {
                return RedirectToAction("Index", "Authorize");
            }

            var userIntId = Convert.ToInt32(userId);
            var cartItems = _context.CartItems
                .Where(c => c.ClientId == userIntId)
                .Include(c => c.Product)
                .ToList();


            return View(cartItems);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var item = _context.CartItems.Find(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }




    }
}
