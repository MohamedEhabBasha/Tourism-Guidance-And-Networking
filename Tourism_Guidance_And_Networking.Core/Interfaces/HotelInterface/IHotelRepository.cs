

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<ICollection<HotelOutputDTO>> GetAllHotels();
        Task<HotelOutputDTO> GetHotelByIdAsync(int id);
        Task<ICollection<HotelOutputDTO>> GetHotelByNameAsync(string name);
        Task<ICollection<HotelOutputDTO>> FilterByRate(double star);
        Task<Hotel> CreateHotelAsync(HotelDTO hotelDTO);

        HotelOutputDTO UpdateHotel(int hotelId, HotelDTO hotelDTO);
        //HotelOutputDTO ToHotelOutputDto(Hotel hotel);
        bool DeleteHotel(int id);
    }
}
