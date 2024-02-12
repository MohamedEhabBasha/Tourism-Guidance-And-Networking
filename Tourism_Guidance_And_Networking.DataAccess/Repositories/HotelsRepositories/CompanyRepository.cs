

using Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.HotelsRepositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public CompanyRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.companyImagesPath}";
        }
    }
}
