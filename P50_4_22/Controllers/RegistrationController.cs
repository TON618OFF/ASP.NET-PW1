using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using P50_4_22.Models;

namespace P50_4_22.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly BulkinKeysContext _context;

        public RegistrationController(BulkinKeysContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            string ClientLogin,
            string ClientPassword,
            string ClientSurname,
            string ClientName,
            string ClientMiddleName,
            string Email,
            string PhoneNumber,
            int ClientAddress_ID)
        {
            // Проверка на существование пользователя с таким же логином, email или номером телефона
            if (await _context.Clients.AnyAsync(u =>
                u.ClientLogin == ClientLogin ||
                u.Email == Email ||
                u.PhoneNumber == PhoneNumber))
            {
                ViewBag.ErrorMessage = "Пользователь с таким логином, email или номером телефона уже существует.";
                return View("Index");
            }

            // Получение роли Customer
            var customerRoleId = await _context.Roles
                .Where(r => r.RoleName == "Customer")
                .Select(r => r.IdRole)
                .FirstOrDefaultAsync();

            if (customerRoleId == 0)
            {
                ViewBag.ErrorMessage = "Роль 'Customer' не найдена в системе. Обратитесь к администратору.";
                return View("Index");
            }

            // Хеширование пароля
            string hashedPassword = HashPassword(ClientPassword);

            // Создание нового клиента
            var client = new Client
            {
                ClientLogin = ClientLogin,
                ClientPassword = hashedPassword,
                ClientSurname = ClientSurname,
                ClientName = ClientName,
                ClientMiddleName = ClientMiddleName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                ClientAddressId = ClientAddress_ID,
                RoleId = customerRoleId // Устанавливаем роль 'Customer'
            };

            // Добавление клиента в базу данных
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Перенаправление на главную страницу
            return RedirectToAction("Index", "Home");
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
