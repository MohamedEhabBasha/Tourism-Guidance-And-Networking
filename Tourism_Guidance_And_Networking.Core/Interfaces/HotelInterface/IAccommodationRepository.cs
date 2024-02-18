

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface IAccommodationRepository : IBaseRepository<Accommodation>
    {
        Task<ICollection<Accommodation>> GetAccommodationsByCompanyIdAsync(int companyId);
        Task<ICollection<Accommodation>> GetAccommodationsByTypeAsync(string type, int companyId);
        Task<Accommodation> CreateAccommodationAsync(AccommodationDTO accommodationDTO);

        Task<Accommodation?> UpdateAccommodation(int accommodationId, AccommodationDTO accommodationDTO);

        Task<bool> TypeExistAsync(string type);
        bool DeleteAccommodation(int id);
    }
}
