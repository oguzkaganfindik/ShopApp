using ShopApp.Business.Dtos;
using ShopApp.Business.Services;
using ShopApp.Data.Entities;
using ShopApp.Data.Repositories;

namespace ShopApp.Business.Managers
{
    public class CategoryManager : ICategoryService
    {
        private readonly IRepository<CategoryEntity> _categoryRepository;

        public CategoryManager(IRepository<CategoryEntity> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public bool AddCategory(CategoryAddDto categoryAddDto)
        {
            var hasCategory = _categoryRepository.GetAll(x => x.Name.ToLower() == categoryAddDto.Name.ToLower()).ToList();

            if(hasCategory.Any()) // hasCategory.Count != 0
            {
                return false;
                // Bu isimde kategori zaten mevcutsa, ekleme yapmayacağımdan geriye false dönüyorum.
            }

            var entity = new CategoryEntity()
            {
                Name = categoryAddDto.Name,
                Description = categoryAddDto.Description
            };

            _categoryRepository.Add(entity);
            return true;
        }

        public List<CategoryListDto> GetCategories()
        {
            var categoryEntities = _categoryRepository.GetAll().OrderBy(x => x.Name);
            var categoryDtoList = categoryEntities.Select(x => new CategoryListDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();
            // Her bir entity için bir adet CategoryListDto newliyor. Veri aktarımı gerçekleşip listeye çeviriyor.

            return categoryDtoList;
        }
    }
}
