using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Dtos;
using ShopApp.Business.Services;
using ShopApp.WebUI.Areas.Admin.Models;

namespace ShopApp.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult List()
        {
            var categoryDtoList = _categoryService.GetCategories();
            var viewModel = categoryDtoList.Select(x => new CategoryListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description?.Length > 20 ? x.Description?.Substring(0, 20) + "..." : x.Description
            }).ToList();

            return View(viewModel);
        }

        public IActionResult New()
        {
            // Eğer ekleme ve güncelleme işlemleri için aynı formu kullanacaksak bu ayrım id üzerinden yapılacağından form mutlaka bir model ile açılmalı.
            return View("Form", new CategoryFormViewModel());
        }

        [HttpPost]
        public IActionResult Save(CategoryFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", formData);
            }

            if (formData.Id == 0) // Ekleme işlemi
            {
                var categoryAddDto = new CategoryAddDto()
                {
                    Name = formData.Name.Trim(),
                    Description = formData.Description?.Trim()
                };

                var result = _categoryService.AddCategory(categoryAddDto);

                if (result)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.ErrorMessage = "Bu isimde bir kategori zaten mevcut.";
                    return View("Form", formData);

                    // View dönüyorsam ViewBag çalışır.
                    // RedirectToAction ile mesaj döneceksem TempData[..] kullanmalıyım.
                }
            }
            else // Güncelleme işlemi
            {
                
            }

            return Ok(); // Silinecek! sadece save action hata vermesin diye şimdilik eklendi.
        }
    }
}
