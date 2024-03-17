using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Dtos;
using ShopApp.Business.Services;
using ShopApp.WebUI.Models;
using System.Security.Claims;

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

            var result = _userService.AddUser(userAddDto);

            if (result.IsSucceed)
            {

                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return View(formData);
            }

        }

        [HttpGet]
        [Route("GirisYap")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("GirisYap")]
        public async Task<IActionResult> Login(LoginViewModel formData)
        {
            if(!ModelState.IsValid) 
            {
                return RedirectToAction("Index", "Home");
            }

            var loginDto = new UserLoginDto()
            {
                Email = formData.Email,
                Password = formData.Password
            };

            var userInfo = _userService.LoginUser(loginDto);

            if(userInfo is null)
            {
                // Uyarı mesajı verilebilir(eklenebilir).
                return RedirectToAction("Index", "Home");
            }

            // Buraya kadar geldiyse demek ki oturum açabilirim.

            var claims = new List<Claim>();

            claims.Add(new Claim ("id" , userInfo.Id.ToString()));
            claims.Add(new Claim ("email" , userInfo.Email));
            claims.Add(new Claim ("firstName" , userInfo.FirstName));
            claims.Add(new Claim ("lastName" , userInfo.LastName));
            claims.Add(new Claim ("userType" , userInfo.UserType.ToString()));

            // Yetkilendirme (Authorization için) gerekli olan alttaki kod
            claims.Add(new Claim(ClaimTypes.Role, userInfo.UserType.ToString())); // ClaimTypes.Role -> .Net içerisinde Authorization mekanizması ile paralel çalışacak.

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var autProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(48))
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), autProperties);

            TempData["SuccessMessage"] = "Kullanıcı girişi yapıldı.";

            return RedirectToAction("Index", "Home");
        }


        [Route("CikisYap")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            TempData["SuccessMessage"] = "Oturum Sonlandırıldı.";
            return RedirectToAction("Index", "Home");
        }
    }
}
