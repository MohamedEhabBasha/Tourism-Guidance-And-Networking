

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface IAccommodationRepository : IBaseRepository<Accommodation>
    {
        Task<ICollection<AccommodationOutputDTO>> GetAllAccommodationsAsync();
        Task<AccommodationOutputDTO> GetAccommodationByIdAsync(int accomId);
        Task<PaginationDTO<AccommodationOutputDTO>> GetAccommodationsByCompanyIdAsync(int pageNumber, int pageSize, int companyId);
        Task<PaginationDTO<AccommodationOutputDTO>> GetAccommodationsByTypeAsync(int pageNumber, int pageSize, string type);
        Task<PaginationDTO<AccommodationOutputDTO>> FilterByPrice(int pageNumber, int pageSize, double minPrice, double maxPrice);
        Task<PaginationDTO<AccommodationOutputDTO>> FilterByRate(int pageNumber, int pageSize, double star);
        Task<PaginationDTO<AccommodationOutputDTO>> GetPaginatedAccommodationAsync(int pageNumber, int pageSize);
        Task<Accommodation> CreateAccommodationAsync(AccommodationDTO accommodationDTO);

        AccommodationOutputDTO UpdateAccommodation(int accommodationId, AccommodationDTO accommodationDTO);

        Task<bool> TypeExistAsync(string type);
        bool DeleteAccommodation(int id);
        static abstract AccommodationOutputDTO ToAccommodationOutputDto(Accommodation accommodation);
    }
}
