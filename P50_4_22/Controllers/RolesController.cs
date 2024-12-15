using Microsoft.AspNetCore.Mvc;
using P50_4_22.Models;
using System.Linq;

namespace P50_4_22.Controllers
{
    public class RolesController : Controller
    {
        private readonly BulkinKeysContext _context;

        public RolesController(BulkinKeysContext context)
        {
            _context = context;
        }

        // GET: Roles/Index
        public IActionResult Index()
        {
            var roles = _context.Roles.ToList(); // Получение всех ролей из базы данных
            return View(roles); // Передача списка ролей в представление
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Add(role);
                _context.SaveChanges(); // Сохранение изменений в базе данных
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public IActionResult Edit(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Update(role);
                _context.SaveChanges(); // Сохранение изменений в базе данных
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public IActionResult Delete(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var role = _context.Roles.Find(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges(); // Удаление записи из базы данных
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
