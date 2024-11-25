using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using P50_4_22.Models;

namespace P50_4_22.Controllers
{
    public class AuthorizeController : Controller
    {

        public BulkinKeysContext _context;

        public AuthorizeController(BulkinKeysContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authorization(string email, string password)
        {
            string hashedPassword = HashPassword(password);

            var user = _context.Clients.FirstOrDefault(u => u.ClientLogin == email && u.ClientPassword == hashedPassword);
            if (user != null)
            {
                var claim = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.ClientPassword),
                    new Claim(ClaimTypes.Email, user.ClientLogin)
                };

                var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Îøèáî÷êà";
            return View();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

    }
}
