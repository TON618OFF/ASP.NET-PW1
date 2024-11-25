﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register(string ClientLogin, string ClientPassword, string ClientSurname, string ClientName, 
            string ClientMiddleName, string Email, string PhoneNumber, int ClientAddress_ID)
        {
            // Проверяем, существует ли пользователь с таким Email
            if (await _context.Clients.AnyAsync(u => u.ClientLogin == ClientLogin))
            {
                ViewBag.ErrorMessage = "Пользователь с таким email уже существует";
                return View("Index");
            }

            // Хэшируем пароль
            string hashedPassword = HashPassword(ClientPassword);

            // Создаем нового пользователя
            var client = new Client
            {
                ClientLogin = ClientLogin,
                ClientPassword = hashedPassword,
                ClientSurname = ClientSurname,
                ClientName = ClientName,
                ClientMiddleName = ClientMiddleName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                ClientAddressId = ClientAddress_ID
                //RoleId = 1 убрать комментарий, когда будет готова таблица с ролями.
            };

            // Сохраняем пользователя в базе данных
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Перенаправляем на главную страницу или страницу успешной регистрации
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