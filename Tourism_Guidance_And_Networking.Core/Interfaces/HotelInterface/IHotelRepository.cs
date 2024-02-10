

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<Hotel> GetHotelByNameAsync(string name);

        Task<ICollection<Room>> GetRoomsByIdAsync(int hotelId);

        Task<ICollection<Room>> GetRoomsByTypeAsync(string type,int hotelId);

        Task<Hotel> CreateHotelAsync(HotelDTO hotelDTO);

        Task<Hotel?> UpdateHotel(HotelDTO hotelDTO);

        bool DeleteHotel(int id);
    }
}
