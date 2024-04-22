
namespace Tourism_Guidance_And_Networking.Core.Interfaces
{
    public interface IImageService
    {
        Tuple<int, string> SaveImage(IFormFile imageFile, string _imagePath);
        void DeleteImage(string imageFileName, string _imagePath);
    }
}
