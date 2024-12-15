using Microsoft.AspNetCore.Mvc;
using P50_4_22.Models;
using Microsoft.EntityFrameworkCore;

namespace P50_4_22.Controllers
{
    public class ClientsAddressController : Controller
    {
        private readonly BulkinKeysContext _context;

        public ClientsAddressController(BulkinKeysContext context)
        {
            _context = context;
        }

        // GET: ClientsAddress
        public async Task<IActionResult> Index()
        {
            var clientsAddresses = await _context.ClientsAddresses.ToListAsync();
            return View(clientsAddresses);
        }

        // GET: ClientsAddress/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientsAddress = await _context.ClientsAddresses
                .FirstOrDefaultAsync(m => m.IdClientAddress == id);
            if (clientsAddress == null)
            {
                return NotFound();
            }

            return View(clientsAddress);
        }

        // GET: ClientsAddress/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClientsAddress/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressLocation,AddressCity,AddressPostalCode,AddressCountry")] ClientsAddress clientsAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientsAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientsAddress);
        }

        // GET: ClientsAddress/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientsAddress = await _context.ClientsAddresses.FindAsync(id);
            if (clientsAddress == null)
            {
                return NotFound();
            }
            return View(clientsAddress);
        }

        // POST: ClientsAddress/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdClientAddress,AddressLocation,AddressCity,AddressPostalCode,AddressCountry")] ClientsAddress clientsAddress)
        {
            if (id != clientsAddress.IdClientAddress)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientsAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientsAddressExists(clientsAddress.IdClientAddress))
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
            return View(clientsAddress);
        }

        // GET: ClientsAddress/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientsAddress = await _context.ClientsAddresses
                .FirstOrDefaultAsync(m => m.IdClientAddress == id);
            if (clientsAddress == null)
            {
                return NotFound();
            }

            return View(clientsAddress);
        }

        // POST: ClientsAddress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientsAddress = await _context.ClientsAddresses.FindAsync(id);
            if (clientsAddress != null)
            {
                _context.ClientsAddresses.Remove(clientsAddress);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool ClientsAddressExists(int id)
        {
            return _context.ClientsAddresses.Any(e => e.IdClientAddress == id);
        }
    }
}
