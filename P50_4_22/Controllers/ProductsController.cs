using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;

public class ProductsController : Controller
{
    private readonly BulkinKeysContext _context;

    public ProductsController(BulkinKeysContext context)
    {
        _context = context;
    }

    // GET: Products
    public IActionResult Index()
    {
        var products = _context.Products.Include(p => p.Category).ToList();
        return View(products);
    }

    // GET: Products/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _context.Products
            .Include(p => p.Category)
            .FirstOrDefault(m => m.IdProduct == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "IdCategory", "CategoryName");
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("ProductName,ProductImage,ProductPrice,ProductDescription,ProductAmount,CategoryId")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "IdCategory", "CategoryName", product.CategoryId);
        return View(product);
    }

    // GET: Products/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }

        ViewData["CategoryId"] = new SelectList(_context.Categories, "IdCategory", "CategoryName", product.CategoryId);
        return View(product);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("IdProduct,ProductName,ProductImage,ProductPrice,ProductDescription,ProductAmount,CategoryId")] Product product)
    {
        if (id != product.IdProduct)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.IdProduct))
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

        ViewData["CategoryId"] = new SelectList(_context.Categories, "IdCategory", "CategoryName", product.CategoryId);
        return View(product);
    }

    // GET: Products/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = _context.Products
            .Include(p => p.Category)
            .FirstOrDefault(m => m.IdProduct == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.IdProduct == id);
    }
}
