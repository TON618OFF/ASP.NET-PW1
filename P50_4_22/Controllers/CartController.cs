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

            var cartItems = _context.CartItems
                .Where(c => c.ClientId == user)
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

            var userIntId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIntId) || !int.TryParse(userIntId, out int user))
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

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.ClientId == user);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = amount,
                    Price = product.ProductPrice * amount,
                    ClientId = user
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
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Cart()
        {
            var userId = User.Identity?.Name;

            if (userId == null)
            {
                return RedirectToAction("Index", "Authorize");
            }

            var userIntId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIntId) || !int.TryParse(userIntId, out int user))
            {
                return RedirectToAction("Index", "Authorize");
            }

            var cartItems = _context.CartItems
                .Where(c => c.ClientId == user)
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

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityModel model)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int user))
            {
                return RedirectToAction("Index", "Authorize");
            }

            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.IdCartItem == model.Id && ci.ClientId == user);

            if (cartItem == null)
            {
                return NotFound();
            }

            if (model.Quantity < 1 || model.Quantity > cartItem.Product.ProductAmount)
            {
                return BadRequest("Недопустимое количество товара.");
            }

            cartItem.Quantity = model.Quantity;
            cartItem.Price = cartItem.Quantity * cartItem.Product.ProductPrice;
            await _context.SaveChangesAsync();

            var itemTotal = cartItem.Price;
            var total = _context.CartItems
                .Where(c => c.ClientId == user)
                .Sum(c => c.Price);

            return Json(new { itemTotal, total });
        }
    }

    public class UpdateQuantityModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
