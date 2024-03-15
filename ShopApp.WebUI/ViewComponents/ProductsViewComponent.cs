using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Services;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.ViewComponents
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke(int? categoryId = null)
        {
            var productDtos = _productService.GetProductsByCategoryId(categoryId);

            var viewModel = productDtos.Select(x => new ProductListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                CategoryName = x.CategoryName,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                ImagePath = x.ImagePath
            }).ToList();

            return View(viewModel);
        }
    }
}