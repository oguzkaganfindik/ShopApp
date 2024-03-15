using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Services;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.ViewComponents
{
    // CategoriesViewComponent harici bir controller gibi düşünülebilir. İçerisinde 1 tane metot olacak (action gibi) --> Invoke
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        public CategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var categoryDtos = _categoryService.GetCategories();

            var viewModel = categoryDtos.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();

            return View(viewModel);
        }
    }
}
