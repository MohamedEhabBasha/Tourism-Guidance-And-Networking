
namespace Tourism_Guidance_And_Networking.DataAccess.Repositories
{
    public class ImageService : IImageService
    {
        protected ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public ImageService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public Tuple<int, string> SaveImage(IFormFile imageFile, string _imagePath)
        {
            try
            {
                var contentPath = _environment.WebRootPath;

                var path = Path.Combine(contentPath, _imagePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                // we are trying to create a unique filename here
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }
        public void DeleteImage(string imageFileName, string _imagePath)
        {
            var contentPath = _environment.WebRootPath;
            var path = Path.Combine(contentPath, _imagePath, imageFileName);
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
