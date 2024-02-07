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
    }
}
