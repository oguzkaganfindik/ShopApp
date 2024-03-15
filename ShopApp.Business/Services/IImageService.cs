using Microsoft.AspNetCore.Http;

namespace ShopApp.Business.Services
{
    public interface IImageService
    {
        string Image(IFormFile formFile, string filePath, out string errorMessage);
    }
}
