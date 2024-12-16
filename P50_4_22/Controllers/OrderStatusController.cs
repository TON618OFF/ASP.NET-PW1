using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;

public class OrderStatusesController : Controller
{
    private readonly BulkinKeysContext _context;

    public OrderStatusesController(BulkinKeysContext context)
    {
        _context = context;
    }

    // Отображение списка всех статусов заказов
    public IActionResult Index()
    {
        var statuses = _context.OrderStatuses.ToList();
        return View(statuses);
    }

    // Отображение деталей статуса заказа
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var orderStatus = _context.OrderStatuses
            .FirstOrDefault(m => m.IdStatus == id);

        if (orderStatus == null)
        {
            return NotFound();
        }

        return View(orderStatus);
    }

    // Отображение формы для создания нового статуса заказа
    public IActionResult Create()
    {
        return View();
    }

    // Обработка POST-запроса для создания нового статуса заказа
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("StatusName")] OrderStatus orderStatus)
    {
        if (ModelState.IsValid)
        {
            _context.Add(orderStatus);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(orderStatus);
    }

    // Отображение формы для редактирования существующего статуса заказа
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var orderStatus = _context.OrderStatuses.Find(id);
        if (orderStatus == null)
        {
            return NotFound();
        }
        return View(orderStatus);
    }

    // Обработка POST-запроса для редактирования статуса заказа
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("IdStatus,StatusName")] OrderStatus orderStatus)
    {
        if (id != orderStatus.IdStatus)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(orderStatus);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderStatusExists(orderStatus.IdStatus))
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
        return View(orderStatus);
    }

    // Отображение страницы для удаления статуса заказа
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var orderStatus = _context.OrderStatuses
            .FirstOrDefault(m => m.IdStatus == id);

        if (orderStatus == null)
        {
            return NotFound();
        }

        return View(orderStatus);
    }

    // Обработка POST-запроса для подтверждения удаления статуса заказа
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var orderStatus = _context.OrderStatuses.Find(id);
        if (orderStatus != null)
        {
            _context.OrderStatuses.Remove(orderStatus);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    // Проверка существования статуса заказа по Id
    private bool OrderStatusExists(int id)
    {
        return _context.OrderStatuses.Any(e => e.IdStatus == id);
    }
}
