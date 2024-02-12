

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
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
        public Task<Company> GetCompanyByNameAsync(string name)
        {
            throw new NotImplementedException();
        } 
        public Task<Company> CreateCompanyAsync(CompanyDTO companyDTO)
        {
            throw new NotImplementedException();
        }
        public Task<Company?> UpdateCompany(int companyId, CompanyDTO companyDTO)
        {
            throw new NotImplementedException();
        }
        public bool DeleteCompany(int id)
        {
            throw new NotImplementedException();
        }

    }
}
