using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Dtos;
using ShopApp.Business.Services;
using ShopApp.WebUI.Areas.Admin.Models;
using SixLabors.ImageSharp;

namespace ShopApp.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImageProcessingService _imageProcessingService;
        private readonly IWebHostEnvironment _environment;
        private readonly IImageService _ImageManager;

        public ProductController(IProductService productService, ICategoryService categoryService, IImageProcessingService imageProcessingService, IWebHostEnvironment environment, IImageService ImageManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageProcessingService = imageProcessingService;
            _environment = environment;
            _ImageManager = ImageManager;
        }

        [HttpGet]
        public IActionResult List()
        {
            var productDtoList = _productService.GetProducts();

            var viewModel = productDtoList.Select(x => new ProductListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryName = x.CategoryName,
                ImagePath = x.ImagePath

            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryService.GetCategories();
            return View("Create", new ProductFormViewModel());
        }

        [HttpPost]
        public IActionResult Create(ProductFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetCategories();
                return View("Create", formData);
            }

            string newFileName = "";
            string filePath = "";
            string errorMessage;

            newFileName = _ImageManager.Image(formData.File, filePath, out errorMessage);
            // Burada Image metodunu çağırarak newFileName ve filePath değişkenlerine değer atıyoruz.

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ImageErrorMessage = errorMessage;
                ViewBag.Categories = _categoryService.GetCategories();
                return View("Create", formData);
            }

            var productAddDto = new ProductAddDto()
            {
                Name = formData.Name,
                Description = formData.Description,
                UnitPrice = formData.UnitPrice,
                UnitsInStock = formData.UnitsInStock,
                CategoryId = formData.CategoryId,
                ImagePath = newFileName
            };

            _productService.AddProduct(productAddDto);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var updateProductDto = _productService.GetProductById(id);

            var viewModel = new ProductFormViewModel()
            {
                Id = updateProductDto.Id,
                Name = updateProductDto.Name,
                Description = updateProductDto.Description,
                UnitPrice = updateProductDto.UnitPrice,
                UnitsInStock = updateProductDto.UnitsInStock,
                CategoryId = updateProductDto.CategoryId,
            };

            ViewBag.ImagePath = updateProductDto.ImagePath;

            ViewBag.Categories = _categoryService.GetCategories();
            return View("Update", viewModel);
        }


        [HttpPost]
        public IActionResult Update(ProductFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetCategories();
                return View("Update", formData);
            }

            string newFileName = "";
            string filePath = "";
            string errorMessage;

            newFileName = _ImageManager.Image(formData.File, filePath, out errorMessage);
            // Burada Image metodunu çağırarak newFileName ve filePath değişkenlerine değer atıyoruz.

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ImageErrorMessage = errorMessage;
                ViewBag.Categories = _categoryService.GetCategories();
                return View("Update", formData);
            }

            var productUpdateDto = new ProductUpdateDto()
            {
                Id = formData.Id,
                Name = formData.Name,
                Description = formData.Description,
                UnitPrice = formData.UnitPrice,
                UnitsInStock = formData.UnitsInStock,
                CategoryId = formData.CategoryId,
                ImagePath = newFileName
            };

            _productService.UpdateProduct(productUpdateDto);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);

            return RedirectToAction("List");
        }
    }
}
