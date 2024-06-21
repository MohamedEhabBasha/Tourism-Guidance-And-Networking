


using Microsoft.AspNetCore.Mvc;
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
        public async Task<ICollection<CompanyOutputDTO>> GetAllCompaniesAsync()
        {
            return await _context.Companies
                .Select(companyDTO => ToCompanyOutputDto(companyDTO))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<CompanyOutputDTO> GetCompanyById(int id)
        {
            var company = await _context.Companies.SingleAsync(c => c.Id == id);

            var companyDto = ToCompanyOutputDto(company);

            return companyDto;
        }
        public async Task<CompanyOutputDTO?> GetCompanyByNameAsync(string name)
        {
            CompanyOutputDTO? company = await _context.Companies
            .Select(companyDTO => ToCompanyOutputDto(companyDTO))
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name.Trim().ToLower().Contains(name));

            if (company is null)
                return null;
            return company;
        }
        public async Task<ICollection<CompanyOutputDTO>> FilterByRate(double star)
        {
            return await _context.Companies
                .Where(c => (c.Rating <= star * 2.0 && c.Rating >= (star - 1) * 2.0))
                .Select(c => ToCompanyOutputDto(c))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Company> CreateCompanyAsync(CompanyDTO companyDTO)
        {

            Company company = new()
            {
                Name = companyDTO.Name,
                Address = companyDTO.Address,
                Rating = companyDTO.Rating,
                Reviews = companyDTO.Reviews,
                ApplicationUserId = companyDTO.ApplicationUserId

            };
            var fileResult = _imageService.SaveImage(companyDTO.ImagePath, _imagesPath);

            if (fileResult.Item1 == 1)
            {
                company.Image = fileResult.Item2;
            }

            return await AddAsync(company);
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
            CompanyOutputDTO companyOutputDTO = ToCompanyOutputDto(company);
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
        private  CompanyOutputDTO ToCompanyOutputDto(Company company)
        {
            CompanyOutputDTO companyOutputDTO = new()
            {
                ID = company.Id,
                Name = company.Name,
                Address = company.Address,
                Rating = company.Rating,
                Reviews = company.Reviews,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{company.Image}"
            };
            return companyOutputDTO;
        }
    }
}
