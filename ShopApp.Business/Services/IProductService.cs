using ShopApp.Business.Dtos;

namespace ShopApp.Business.Services
{
    public interface IProductService
    {
        void AddProduct(ProductAddDto productAddDto);

        List<ProductListDto> GetProducts();

        ProductUpdateDto GetProductById(int id);

        void UpdateProduct(ProductUpdateDto productUpdateDto);

        void DeleteProduct(int id);
    }
}
