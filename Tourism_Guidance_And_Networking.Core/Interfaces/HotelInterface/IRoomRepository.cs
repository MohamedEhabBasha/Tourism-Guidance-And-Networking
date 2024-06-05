

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<ICollection<RoomOutputDTO>> SearchRoomByNameAsync(string name);
        Task<RoomOutputDTO> GetRoomById(int id);
        Task<ICollection<RoomOutputDTO>> GetRoomsByHotelIdAsync(int hotelId);
        Task<ICollection<RoomOutputDTO>> GetRoomsByTypeAsync(string type, int hotelId);
        Task<RoomOutputDTO> CreateRoomAsync(RoomDTO roomlDTO);

        Task<RoomOutputDTO?> UpdateRoom(int roomId, RoomDTO roomDTO);

        bool DeleteRoom(int id);
        bool TypeExist(string type);
    }
}
