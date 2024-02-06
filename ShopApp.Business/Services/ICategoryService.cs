using ShopApp.Business.Dtos;

namespace ShopApp.Business.Services
{
    public interface ICategoryService
    {
        bool AddCategory(CategoryAddDto categoryAddDto);
        List<CategoryListDto> GetCategories();
    }
}
