

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.HotelsRepositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public RoomRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : base(context) 
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.roomImagesPath}";
        }
        public async Task<ICollection<Room>> SearchRoomByNameAsync(string name)
        {
            return await _context.Rooms
                .Where(c => c.Type.Trim().ToLower().Contains(name) || c.Info!.Trim().ToLower().Contains(name))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Room> CreateRoomAsync(RoomDTO roomDTO)
        {
            string coverName = await SaveCover(roomDTO.ImagePath, _imagesPath);

            Room room = new()
            {
                 Type = roomDTO.Type,
                 Price = roomDTO.Price,
                 Taxes = roomDTO.Taxes,
                 Info = roomDTO.Info,
                 Capicity = roomDTO.Capicity,
                 HotelId = roomDTO.HotelId,
                 Image = coverName
            };

            return await AddAsync(room);
        }
        public async Task<Room?> UpdateRoom(RoomDTO roomDTO)
        {
            var room = _context.Rooms.SingleOrDefault(c => c.Id == roomDTO.Id);
            if (room is null) { return null; }

            bool hasNewCover = roomDTO.ImagePath is not null;
            bool equal = Equal(room, roomDTO);
            var oldCover = room.Image;

            room.Type = roomDTO.Type;
            room.Price = roomDTO.Price;
            room.Taxes = roomDTO.Taxes;
            room.Capicity = roomDTO.Capicity;
            room.Info = roomDTO.Info;
            room.HotelId = roomDTO.HotelId;

            if (hasNewCover)
            {
                room.Image = await SaveCover(roomDTO.ImagePath!, _imagesPath);
                equal = oldCover == room.Image;
            }
            if (!equal)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }

                return room;
            }
            else
            {
                //var cover = Path.Combine(_imagesPath, touristPlace.Image);
                //File.Delete(cover);

                return room;
            }
        }
        public bool DeleteRoom(int id)
        {
            var room = _context.Rooms.SingleOrDefault(c => c.Id == id);

            if (room == null)
            {
                return false;
            }
            var cover = Path.Combine(_imagesPath, room.Image);
            File.Delete(cover);

            Delete(room);

            return true;
        }
        public bool TypeExist(string type)
        {
            return _context.Rooms.SingleOrDefault(c => c.Type == type) != null;
        }
        private static bool Equal(Room room, RoomDTO roomDTO)
        {
            if (room.Type != roomDTO.Type || room.Price != roomDTO.Price || room.Taxes != roomDTO.Taxes ||
                room.Info != roomDTO.Info || room.Capicity != roomDTO.Capicity || room.HotelId != roomDTO.HotelId 
                )
                return false;
            return true;
        }
    }
}
