using ShopApp.Business.Dtos;
using ShopApp.Business.Services;
using ShopApp.Data.Entities;
using ShopApp.Data.Repositories;

namespace ShopApp.Business.Managers
{
    public class ProductManager : IProductService
    {
        private readonly IRepository<ProductEntity> _productRepository;

        public ProductManager(IRepository<ProductEntity> productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddProduct(ProductAddDto productAddDto)
        {
            var entity = new ProductEntity()
            {
                Name = productAddDto.Name,
                Description = productAddDto.Description,
                UnitPrice = productAddDto.UnitPrice,
                UnitInStock = productAddDto.UnitsInStock,
                CategoryId = productAddDto.CategoryId,
                ImagePath = productAddDto.ImagePath,
            };

            _productRepository.Add(entity);
        }

        public ProductUpdateDto GetProductById(int id)
        {
            var entity = _productRepository.GetById(id);

            var productUpdateDto = new ProductUpdateDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                UnitPrice = entity.UnitPrice,
                UnitsInStock = entity.UnitInStock,
                CategoryId = entity.CategoryId,
                ImagePath = entity.ImagePath,
            };

            return productUpdateDto;
        }

        public List<ProductListDto> GetProducts()
        {
            var productEntites = _productRepository.GetAll().OrderBy(x => x.Category.Name).ThenBy(x => x.Name);
            // Önce kategori adına, sonra kategori içinde ürün isimlerine göre sıralıyorum.

            var productDtoList = productEntites.Select(x => new ProductListDto()
            {
                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice,
                UnitInStock = x.UnitInStock,
                CategoryName = x.Category.Name,
                ImagePath = x.ImagePath,
            }).ToList();

            return productDtoList;
        }
    }
}
