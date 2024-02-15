


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
        public async Task<Company?> GetCompanyByNameAsync(string name)
        {
            Company? company = await _context.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.Name.Trim().ToLower().Contains(name));

            if (company is null)
                return null;
            return company;
        } 
        public async Task<Company> CreateCompanyAsync(CompanyDTO companyDTO)
        {
            string coverName = await SaveCover(companyDTO.ImagePath, _imagesPath);

            Company company = new()
            {
                Name = companyDTO.Name,
                Address = companyDTO.Address,
                Rating = companyDTO.Rating,
                Reviews = companyDTO.Reviews,
                Image = coverName
            };

            return await AddAsync(company);
        }
        public async Task<Company?> UpdateCompany(int companyId, CompanyDTO companyDTO)
        {
            var company = _context.Companies.SingleOrDefault(c => c.Id == companyId);
            if (company is null) { return null; }

            bool hasNewCover = companyDTO.ImagePath is not null;
            bool equal = Equal(company, companyDTO);
            var oldCover = company.Image;

            company.Name = companyDTO.Name;
            company.Address = companyDTO.Address;
            company.Rating = companyDTO.Rating;
            company.Reviews = companyDTO.Reviews;

            if (hasNewCover)
            {
                company.Image = await SaveCover(companyDTO.ImagePath!, _imagesPath);
                equal = oldCover == company.Image;
            }
            if (!equal)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }

                return company;
            }
            else
            {
                //var cover = Path.Combine(_imagesPath, touristPlace.Image);
                //File.Delete(cover);

                return company;
            }
        }
        public bool DeleteCompany(int id)
        {
            var company = _context.Companies.SingleOrDefault(c => c.Id == id);

            if (company == null)
            {
                return false;
            }
            var cover = Path.Combine(_imagesPath, company.Image);
            File.Delete(cover);

            Delete(company);

            return true;
        }
        private static bool Equal(Company company, CompanyDTO companyDTO)
        {
            if (company.Name != companyDTO.Name || company.Address != companyDTO.Address ||
                company.Rating != companyDTO.Rating || company.Reviews != companyDTO.Reviews)
                return false;
            return true;
        }
    }
}
