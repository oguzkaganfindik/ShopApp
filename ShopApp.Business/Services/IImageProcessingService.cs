namespace ShopApp.Business.Services
{
    public interface IImageProcessingService
    {
        void ResizeImage(Stream sourceStream, string destinationImagePath, int width, int height);
    }
}
