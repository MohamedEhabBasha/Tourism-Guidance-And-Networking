

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface IAccommodationRepository : IBaseRepository<Accommodation>
    {
        Task<ICollection<AccommodationOutputDTO>> GetAllAccommodationsAsync();
        Task<AccommodationOutputDTO> GetAccommodationByIdAsync(int accomId);
        Task<ICollection<AccommodationOutputDTO>> GetAccommodationsByCompanyIdAsync(int companyId);
        Task<ICollection<AccommodationOutputDTO>> GetAccommodationsByTypeAsync(string type, int companyId);
        Task<ICollection<AccommodationOutputDTO>> FilterByPrice(double price);
        Task<ICollection<AccommodationOutputDTO>> FilterByRate(double star);
        Task<Accommodation> CreateAccommodationAsync(AccommodationDTO accommodationDTO);

        AccommodationOutputDTO UpdateAccommodation(int accommodationId, AccommodationDTO accommodationDTO);

        Task<bool> TypeExistAsync(string type);
        bool DeleteAccommodation(int id);
        static abstract AccommodationOutputDTO ToAccommodationOutputDto(Accommodation accommodation);
    }
}
