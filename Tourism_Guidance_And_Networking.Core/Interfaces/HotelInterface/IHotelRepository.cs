

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<ICollection<HotelOutputDTO>> GetAllHotels();
        Task<HotelOutputDTO> GetHotelByIdAsync(int id);
        Task<HotelOutputDTO> GetHotelByNameAsync(string name);

        Task<HotelOutputDTO> CreateHotelAsync(HotelDTO hotelDTO);

        Task<HotelOutputDTO?> UpdateHotel(int hotelId, HotelDTO hotelDTO);

        bool DeleteHotel(int id);
    }
}
