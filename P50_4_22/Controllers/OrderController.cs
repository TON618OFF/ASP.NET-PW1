using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;

public class OrdersController : Controller
{
    private readonly BulkinKeysContext _context;

    public OrdersController(BulkinKeysContext context)
    {
        _context = context;
    }

    // GET: Orders
    public IActionResult Index()
    {
        var orders = _context.Orders
            .Include(o => o.Client)
            .Include(o => o.Status)
            .ToList();
        return View(orders);
    }

    // GET: Orders/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = _context.Orders
            .Include(o => o.Client)
            .Include(o => o.Status)
            .FirstOrDefault(m => m.IdOrder == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // GET: Orders/Create
    public IActionResult Create()
    {
        ViewData["ClientId"] = new SelectList(_context.Clients, "IdClient", "ClientName");
        ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "IdStatus", "StatusName");
        return View();
    }


    // POST: Orders/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("OrderNumber, OrderDate, OrderTotalSum, ClientId, StatusId")] Order order)
    {
        if (ModelState.IsValid)
        {
            _context.Add(order);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClientId"] = new SelectList(_context.Clients, "IdClient", "ClientName", order.ClientId);
        ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "IdStatus", "StatusName", order.StatusId);
        return View(order);
    }

    // GET: Orders/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = _context.Orders.Find(id);
        if (order == null)
        {
            return NotFound();
        }
        ViewData["ClientId"] = new SelectList(_context.Clients, "IdClient", "ClientName", order.ClientId);
        ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "IdStatus", "StatusName", order.StatusId);
        return View(order);
    }

    // POST: Orders/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("IdOrder, OrderNumber, OrderDate, OrderTotalSum, ClientId, StatusId")] Order order)
    {
        if (id != order.IdOrder)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(order);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.IdOrder))
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
        ViewData["ClientId"] = new SelectList(_context.Clients, "IdClient", "ClientName", order.ClientId);
        ViewData["StatusId"] = new SelectList(_context.OrderStatuses, "IdStatus", "StatusName", order.StatusId);
        return View(order);
    }

    // GET: Orders/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = _context.Orders
            .Include(o => o.Client)
            .Include(o => o.Status)
            .FirstOrDefault(m => m.IdOrder == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // POST: Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var order = _context.Orders.Find(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool OrderExists(int id)
    {
        return _context.Orders.Any(e => e.IdOrder == id);
    }
}
