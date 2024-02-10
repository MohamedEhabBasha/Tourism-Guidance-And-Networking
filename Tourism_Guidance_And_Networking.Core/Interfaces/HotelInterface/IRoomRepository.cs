

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<ICollection<Room>> SearchRoomByNameAsync(string name);
        Task<Room> CreateRoomAsync(RoomDTO roomlDTO);

        Task<Room?> UpdateRoom(RoomDTO roomDTO);

        bool DeleteRoom(int id);
        bool TypeExist(string type);
    }
}
