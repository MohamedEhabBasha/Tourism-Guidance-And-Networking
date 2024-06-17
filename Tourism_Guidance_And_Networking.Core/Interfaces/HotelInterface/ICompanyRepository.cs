

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<ICollection<CompanyOutputDTO>> GetAllCompaniesAsync();
        Task<CompanyOutputDTO> GetCompanyById(int id);
        Task<CompanyOutputDTO?> GetCompanyByNameAsync(string name);
        Task<Company> CreateCompanyAsync(CompanyDTO companyDTO);

        Task<CompanyOutputDTO?> UpdateCompany(int companyId, CompanyDTO companyDTO);

        bool DeleteCompany(int id);
    }
}
