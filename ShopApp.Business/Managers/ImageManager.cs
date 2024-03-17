using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ShopApp.Business.Services;

namespace ShopApp.Business.Managers
{

    public class ImageManager : IImageService
    {
        private readonly IHostingEnvironment _environment;
        private readonly IImageProcessingService _imageProcessingService;

        public ImageManager(IHostingEnvironment environment, IImageProcessingService imageProcessingService)
        {
            _environment = environment;
            _imageProcessingService = imageProcessingService;
        }

        public string Image(IFormFile formFile, string filePath, out string errorMessage)
        {
            var newFileName = "";

            errorMessage = "";

            if (formFile != null && formFile.FileName != null) // dosya yüklenmek isteniyorsa
            {
                var allowedFileTypes = new string[] { "image/jpeg", "image/jpg", "image/jfif", "image/avif" };

                var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".jfif", ".avif" };

                var fileContentType = formFile.ContentType;
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formFile.FileName);
                var fileExtension = Path.GetExtension(formFile.FileName);

                if (!allowedFileTypes.Contains(fileContentType) || !allowedFileExtensions.Contains(fileExtension))
                {
                    errorMessage = "Yüklediğiniz dosya " + fileExtension + " uzantısında. Sisteme yalnızca .jpg .jpeg .jfif .avif formatında dosyalar yüklenebilir.";
                }
                else
                {
                    newFileName = fileNameWithoutExtension + "-" + Guid.NewGuid() + fileExtension;
                    var folderPath = Path.Combine("images", "products");
                    var wwwrootFolderPath = Path.Combine(_environment.WebRootPath, folderPath);
                    Directory.CreateDirectory(wwwrootFolderPath);
                    filePath = Path.Combine(wwwrootFolderPath, newFileName);

                    // Resmi boyutlandırma
                    _imageProcessingService.ResizeImage(formFile.OpenReadStream(), filePath, 1800, 1200);
                }
            }

            return newFileName;
        }
    }
}

