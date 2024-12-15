using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P50_4_22.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

public class ClientsController : Controller
{
    private readonly BulkinKeysContext _context;

    public ClientsController(BulkinKeysContext context)
    {
        _context = context;
    }

    // GET: Clients/Index
    public IActionResult Index()
    {
        var clients = _context.Clients
            .Include(c => c.ClientAddress)
            .Include(c => c.Role)
            .ToList();
        return View(clients);
    }

    // GET: Clients/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = _context.Clients
            .Include(c => c.ClientAddress)
            .Include(c => c.Role)
            .FirstOrDefault(m => m.IdClient == id);

        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    // GET: Clients/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Clients/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("ClientLogin,ClientPassword,ClientSurname,ClientName,ClientMiddleName,Email,PhoneNumber,ClientAddressId,RoleId")] Client client)
    {
        if (ModelState.IsValid)
        {
            // Хэшируем пароль
            client.ClientPassword = HashPassword(client.ClientPassword);

            _context.Add(client);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(client);
    }


    // GET: Clients/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = _context.Clients.Find(id);
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }

    // POST: Clients/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("IdClient,ClientLogin,ClientPassword,ClientSurname,ClientName,ClientMiddleName,Email,PhoneNumber,ClientAddressId,RoleId")] Client client)
    {
        if (id != client.IdClient)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (!string.IsNullOrEmpty(client.ClientPassword))
                {
                    client.ClientPassword = HashPassword(client.ClientPassword); // Хэшируем пароль, если он был изменен
                }

                _context.Update(client);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.IdClient))
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
        return View(client);
    }


    // GET: Clients/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = _context.Clients
            .Include(c => c.ClientAddress)
            .Include(c => c.Role)
            .FirstOrDefault(m => m.IdClient == id);

        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    // POST: Clients/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var client = _context.Clients.Find(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }


    private bool ClientExists(int id)
    {
        return _context.Clients.Any(e => e.IdClient == id);
    }

    // Хэшируем пароль с использованием SHA256
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
