using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;

public class CategoriesController : Controller
{
    private readonly BulkinKeysContext _context;

    public CategoriesController(BulkinKeysContext context)
    {
        _context = context;
    }

    // GET: Categories/Index
    public IActionResult Index()
    {
        var categories = _context.Categories.ToList();  // Получаем все категории из базы данных
        return View(categories);
    }

    // GET: Categories/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = _context.Categories
            .FirstOrDefault(m => m.IdCategory == id);  // Ищем категорию по Id

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // GET: Categories/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Categories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("CategoryName")] Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Add(category);  // Добавляем новую категорию
            _context.SaveChanges();  // Сохраняем изменения в базе данных
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // GET: Categories/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = _context.Categories.Find(id);  // Находим категорию по Id
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // POST: Categories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("IdCategory,CategoryName")] Category category)
    {
        if (id != category.IdCategory)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);  // Обновляем категорию в базе данных
                _context.SaveChanges();  // Сохраняем изменения
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.IdCategory))
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
        return View(category);
    }

    // GET: Categories/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = _context.Categories
            .FirstOrDefault(m => m.IdCategory == id);  // Ищем категорию по Id

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // POST: Categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var category = _context.Categories.Find(id);  // Находим категорию по Id
        if (category != null)
        {
            _context.Categories.Remove(category);  // Удаляем категорию
            _context.SaveChanges();  // Сохраняем изменения
        }
        return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.IdCategory == id);  // Проверяем, существует ли категория с данным Id
    }
}
