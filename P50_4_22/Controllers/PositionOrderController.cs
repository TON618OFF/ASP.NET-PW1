using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;

public class PositionOrdersController : Controller
{
    private readonly BulkinKeysContext _context;

    public PositionOrdersController(BulkinKeysContext context)
    {
        _context = context;
    }

    // GET: PositionOrders
    public IActionResult Index()
    {
        var positionOrders = _context.PositionOrders.Include(p => p.Order).Include(p => p.Product).ToList();
        return View(positionOrders);
    }

    // GET: PositionOrders/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var positionOrder = _context.PositionOrders
            .Include(p => p.Order)
            .Include(p => p.Product)
            .FirstOrDefault(m => m.IdPositionOrder == id);

        if (positionOrder == null)
        {
            return NotFound();
        }

        return View(positionOrder);
    }

    // GET: PositionOrders/Create
    public IActionResult Create()
    {
        ViewData["OrderId"] = new SelectList(_context.Orders, "IdOrder", "OrderNumber");
        ViewData["ProductId"] = new SelectList(_context.Products, "IdProduct", "ProductName");
        return View();
    }

    // POST: PositionOrders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("PositionOrderCount, PositionOrderPrice, OrderId, ProductId")] PositionOrder positionOrder)
    {
        if (ModelState.IsValid)
        {
            _context.Add(positionOrder);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewData["OrderId"] = new SelectList(_context.Orders, "IdOrder", "OrderNumber", positionOrder.OrderId);
        ViewData["ProductId"] = new SelectList(_context.Products, "IdProduct", "ProductName", positionOrder.ProductId);
        return View(positionOrder);
    }

    // GET: PositionOrders/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var positionOrder = _context.PositionOrders.Find(id);
        if (positionOrder == null)
        {
            return NotFound();
        }
        ViewData["OrderId"] = new SelectList(_context.Orders, "IdOrder", "OrderNumber", positionOrder.OrderId);
        ViewData["ProductId"] = new SelectList(_context.Products, "IdProduct", "ProductName", positionOrder.ProductId);
        return View(positionOrder);
    }

    // POST: PositionOrders/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("IdPositionOrder, PositionOrderCount, PositionOrderPrice, OrderId, ProductId")] PositionOrder positionOrder)
    {
        if (id != positionOrder.IdPositionOrder)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(positionOrder);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PositionOrderExists(positionOrder.IdPositionOrder))
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
        ViewData["OrderId"] = new SelectList(_context.Orders, "IdOrder", "OrderNumber", positionOrder.OrderId);
        ViewData["ProductId"] = new SelectList(_context.Products, "IdProduct", "ProductName", positionOrder.ProductId);
        return View(positionOrder);
    }

    // GET: PositionOrders/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var positionOrder = _context.PositionOrders
            .Include(p => p.Order)
            .Include(p => p.Product)
            .FirstOrDefault(m => m.IdPositionOrder == id);

        if (positionOrder == null)
        {
            return NotFound();
        }

        return View(positionOrder);
    }

    // POST: PositionOrders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var positionOrder = _context.PositionOrders.Find(id);
        if (positionOrder != null)
        {
            _context.PositionOrders.Remove(positionOrder);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool PositionOrderExists(int id)
    {
        return _context.PositionOrders.Any(e => e.IdPositionOrder == id);
    }
}
