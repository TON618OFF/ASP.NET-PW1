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
        [HttpGet]
        public IActionResult Authorize()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Authorize(string email, string password)
        {
            string hashedPassword = HashPassword(password);

            var user = _context.Clients.FirstOrDefault(u => u.ClientLogin == email && u.ClientPassword == hashedPassword);
            if (user != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.IdClient.ToString()),
                    new Claim(ClaimTypes.Name, user.ClientLogin),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString()) 
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Ошибка авторизации!";
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
