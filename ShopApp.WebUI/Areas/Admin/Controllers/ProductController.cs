using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Services;
using ShopApp.WebUI.Areas.Admin.Models;

namespace ShopApp.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult New()
        {
            ViewBag.Categories = _categoryService.GetCategories();
            return View("Form", new ProductFormViewModel());
        }

        public IActionResult Save(ProductFormViewModel formData)
        {
            if(!ModelState.IsValid) 
            {
                ViewBag.Categories = _categoryService.GetCategories();
                return View("Form", formData);
            }

            var newFileName = "";

            if(formData.File is not null) // dosya yüklenmek isteniyorsa
            {
                var allowedFileTypes = new string[] { "image/jpeg", "image/jpg", "image/jfif", "image/avif" };

                var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".jfif", ".avif" };

                var fileContentType = formData.File.ContentType;
                // dosyanın içeriği

                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formData.File.FileName);
                // dosyanın uzantısız ismi

                var fileExtension = Path.GetExtension(formData.File.FileName);
                // dosyanın uzantısı

                if(!allowedFileTypes.Contains(fileContentType) || !allowedFileExtensions.Contains(fileExtension))
                {
                    ViewBag.ImageErrorMessage = "Yüklediğiniz dosya " + fileExtension + " uzantısında. Sisteme yalnızca .jpg .jpeg .jfif .avif formatında dosyalar yüklenebilir.";
                    ViewBag.Categories = _categoryService.GetCategories();
                    return View("Form", formData);
                }

                newFileName = fileNameWithoutExtension + "-" + Guid.NewGuid() + fileExtension;
                // Aynı isimde iki tane dosya yüklediğimizde hata almamak için her dosyayı birbiriyle asla eşleşmeyecek şekilde isimlendiriyorum. Guid : unique bir string verir.


            }

            return Ok();
        }
    }
}
