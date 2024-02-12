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

        public void DeleteCategory(int id)
        {
            // TODO : Bu category ile eşleşen ürün var mı diye kontrol et, eğer ürün varsa silme işlemi yapılmalı, geriye FALSE dönülmeli ( system message ile hem false hem de hata mesajını dönebiliriz. )

            // Eğer eşleşen ürün yoksa, işlem aynı şekilde repository'e taşınır ve category silinir.

            _categoryRepository.Delete(id);
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

        public CategoryUpdateDto GetCategory(int id)
        {
            var entity = _categoryRepository.GetById(id);

            var categoryUpdateDto = new CategoryUpdateDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };

            return categoryUpdateDto;
        }

        public bool UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            var hasCategory = _categoryRepository.GetAll(x => x.Name.ToLower() == categoryUpdateDto.Name.ToLower() && x.Id != categoryUpdateDto.Id).ToList();
            // Kendisi hariç aynı isimde başka veriyle eşleşiyorsa listeye çekiyorum. Bunu yapmamdaki neden ismi aynı tutup diğer özellikleri değiştirmek istediğimizde kendi verisini çekip zaten mevcut dememesi için.

            if (hasCategory.Any()) // hasCategory.Count != 0
            {
                return false;
                // Aynı isimde kategori olduğunda o isme güncelleme yapmıyorum.
            }

            var entity = _categoryRepository.GetById(categoryUpdateDto.Id);

            entity.Name = categoryUpdateDto.Name;
            entity.Description = categoryUpdateDto.Description;

            _categoryRepository.Update(entity);

            return true;
        }
    }
}
