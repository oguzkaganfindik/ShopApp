using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Dtos;
using ShopApp.Business.Services;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    // Authentication and Authorization
    // (Kimlik Doğrulama - Yetkilendirme)
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("KayitOl")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("KayitOl")]
        public IActionResult Register(RegisterViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var userAddDto = new UserAddDto()
            {
                FirstName = formData.FirstName.Trim(),
                LastName = formData.LastName.Trim(),
                Email = formData.Email.Trim(),
                Password = formData.Password
            };

            return RedirectToAction("Index", "Home");
        }
    }
}
