using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;

public class CartItemsController : Controller
{
    private readonly BulkinKeysContext _context;

    public CartItemsController(BulkinKeysContext context)
    {
        _context = context;
    }

    // Index - список всех элементов корзины
    public IActionResult Index()
    {
        var cartItems = _context.CartItems.Include(c => c.Client).Include(c => c.Product).ToList();
        return View(cartItems);
    }

    // Details - информация о конкретном элементе корзины
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cartItem = _context.CartItems
            .Include(c => c.Client)
            .Include(c => c.Product)
            .FirstOrDefault(m => m.IdCartItem == id);

        if (cartItem == null)
        {
            return NotFound();
        }

        return View(cartItem);
    }

    // Create - форма для создания нового элемента корзины
    public IActionResult Create()
    {
        ViewData["ClientId"] = new SelectList(_context.Clients, "IdClient", "ClientName");
        ViewData["ProductId"] = new SelectList(_context.Products, "IdProduct", "ProductName");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("ClientId,ProductId,Quantity,Price")] CartItem cartItem)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cartItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClientId"] = new SelectList(_context.Clients, "IdClient", "ClientName", cartItem.ClientId);
        ViewData["ProductId"] = new SelectList(_context.Products, "IdProduct", "ProductName", cartItem.ProductId);
        return View(cartItem);
    }

    // Edit - форма для редактирования существующего элемента корзины
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cartItem = _context.CartItems.Find(id);
        if (cartItem == null)
        {
            return NotFound();
        }

        ViewData["ClientId"] = new SelectList(_context.Clients, "IdClient", "ClientName", cartItem.ClientId);
        ViewData["ProductId"] = new SelectList(_context.Products, "IdProduct", "ProductName", cartItem.ProductId);
        return View(cartItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("IdCartItem,ClientId,ProductId,Quantity,Price")] CartItem cartItem)
    {
        if (id != cartItem.IdCartItem)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cartItem);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemExists(cartItem.IdCartItem))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClientId"] = new SelectList(_context.Clients, "IdClient", "ClientName", cartItem.ClientId);
        ViewData["ProductId"] = new SelectList(_context.Products, "IdProduct", "ProductName", cartItem.ProductId);
        return View(cartItem);
    }

    // Delete - подтверждение удаления элемента корзины
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cartItem = _context.CartItems
            .Include(c => c.Client)
            .Include(c => c.Product)
            .FirstOrDefault(m => m.IdCartItem == id);

        if (cartItem == null)
        {
            return NotFound();
        }

        return View(cartItem);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var cartItem = _context.CartItems.Find(id);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool CartItemExists(int id)
    {
        return _context.CartItems.Any(e => e.IdCartItem == id);
    }
}
