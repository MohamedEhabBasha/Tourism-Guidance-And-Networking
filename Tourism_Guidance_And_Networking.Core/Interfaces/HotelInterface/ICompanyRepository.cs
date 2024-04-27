﻿

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<CompanyOutputDTO?> GetCompanyByNameAsync(string name);
        Task<CompanyOutputDTO> CreateCompanyAsync(CompanyDTO companyDTO);

        Task<CompanyOutputDTO?> UpdateCompany(int companyId, CompanyDTO companyDTO);

        bool DeleteCompany(int id);
    }
}
