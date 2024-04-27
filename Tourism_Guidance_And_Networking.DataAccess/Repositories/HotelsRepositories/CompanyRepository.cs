


using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.HotelsRepositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly string _imagesPath;
        public CompanyRepository(ApplicationDbContext context, IImageService imageService) : base(context)
        {
            _context = context;
            _imageService = imageService;
            _imagesPath = FileSettings.companyImagesPath;
        }
        public async Task<CompanyOutputDTO?> GetCompanyByNameAsync(string name)
        {
            CompanyOutputDTO? company = await _context.Companies
            .Select(companyDTO => new CompanyOutputDTO
            {
                    Name = companyDTO.Name,
                    Address = companyDTO.Address,
                    Rating = companyDTO.Rating,
                    Reviews = companyDTO.Reviews,
                    ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{companyDTO.Image}"
            })
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.Trim().ToLower().Contains(name));

            if (company is null)
                return null;
            return company;
        } 
        public async Task<CompanyOutputDTO> CreateCompanyAsync(CompanyDTO companyDTO)
        {

            Company company = new()
            {
                Name = companyDTO.Name,
                Address = companyDTO.Address,
                Rating = companyDTO.Rating,
                Reviews = companyDTO.Reviews
            };
            var fileResult = _imageService.SaveImage(companyDTO.ImagePath, _imagesPath);

            if (fileResult.Item1 == 1)
            {
                company.Image = fileResult.Item2;
            }
            CompanyOutputDTO companyOutputDTO = new()
            {
                Name = companyDTO.Name,
                Address = companyDTO.Address,
                Rating = companyDTO.Rating,
                Reviews = companyDTO.Reviews,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{company.Image}"
            };
            await AddAsync(company);

            return companyOutputDTO;
        }
        public async Task<CompanyOutputDTO?> UpdateCompany(int companyId, CompanyDTO companyDTO)
        {
            var company = await _context.Companies.SingleOrDefaultAsync(c => c.Id == companyId);
            if (company is null) { return null; }

            string oldImage = company.Image;

            if (companyDTO.ImagePath is not null)
            {
                var fileResult = _imageService.SaveImage(companyDTO.ImagePath, _imagesPath);

                if (fileResult.Item1 == 1)
                {
                    company.Image = fileResult.Item2;
                }
            }

            company.Name = companyDTO.Name;
            company.Address = companyDTO.Address;
            company.Rating = companyDTO.Rating;
            company.Reviews = companyDTO.Reviews;

            if (companyDTO.ImagePath is not null)
            {
                _imageService.DeleteImage(oldImage, _imagesPath);
            }
            CompanyOutputDTO companyOutputDTO = new()
            {
                Name = companyDTO.Name,
                Address = companyDTO.Address,
                Rating = companyDTO.Rating,
                Reviews = companyDTO.Reviews,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{company.Image}"
            };
            return companyOutputDTO;
        }
        public bool DeleteCompany(int id)
        {
            var company = _context.Companies.SingleOrDefault(c => c.Id == id);

            if (company == null)
            {
                return false;
            }

            Delete(company);
            _imageService.DeleteImage(company.Image, _imagesPath);

            return true;
        }
    }
}
