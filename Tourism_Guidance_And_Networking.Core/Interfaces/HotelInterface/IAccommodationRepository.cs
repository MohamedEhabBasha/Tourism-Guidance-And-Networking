

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface IAccommodationRepository : IBaseRepository<Accommodation>
    {
        Task<ICollection<Accommodation>> GetAccommodationByCompanyIdAsync(int companyId);
        Task<ICollection<Accommodation>> GetAccommodationByTypeAsync(string type, int companyId);
        Task<Accommodation> CreateAccommodationAsync(AccommodationDTO accommodationDTO);

        Task<Accommodation?> UpdateAccommodation(int accommodationId, AccommodationDTO accommodationDTO);

        bool DeleteAccommodation(int id);
    }
}
