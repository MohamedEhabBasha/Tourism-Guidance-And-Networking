

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Interfaces;
using Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.HotelsRepositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly string _imagesPath;
        public RoomRepository(ApplicationDbContext context, IImageService imageService) : base(context) 
        {
            _context = context;
            _imageService = imageService;
            _imagesPath = FileSettings.roomImagesPath;
        }
        public async Task<RoomOutputDTO> GetRoomById(int id)
        {
            var room = await _context.Rooms.SingleAsync(r => r.Id == id);

            var roomDto = new RoomOutputDTO
            {
                ID = room.Id,
                Type = room.Type,
                Price = room.Price,
                Taxes = room.Taxes,
                Info = room.Info,
                Capicity = room.Capicity,
                HotelId = room.HotelId,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{room.Image}",
                Count = room.Count
            };

            return roomDto;
        }
        public async Task<ICollection<RoomOutputDTO>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(c => c.HotelId == hotelId)
                .AsNoTracking()
                .Select(roomDTO => new RoomOutputDTO
                {
                    ID = roomDTO.Id,
                    Type = roomDTO.Type,
                    Price = roomDTO.Price,
                    Taxes = roomDTO.Taxes,
                    Info = roomDTO.Info,
                    Capicity = roomDTO.Capicity,
                    HotelId = roomDTO.HotelId,
                    ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{roomDTO.Image}",
                    Count = roomDTO.Count
                })
                .ToListAsync();
        }
        public async Task<ICollection<RoomOutputDTO>> GetRoomsByTypeAsync(string type, int hotelId)
        {
            return await _context.Rooms
                .Where(c => c.Type.Trim().ToLower().Contains(type) && c.HotelId == hotelId)
                .Select(roomDTO => new RoomOutputDTO
                {
                    ID = roomDTO.Id,
                    Type = roomDTO.Type,
                    Price = roomDTO.Price,
                    Taxes = roomDTO.Taxes,
                    Info = roomDTO.Info,
                    Capicity = roomDTO.Capicity,
                    HotelId = roomDTO.HotelId,
                    ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{roomDTO.Image}",
                    Count = roomDTO.Count
                })
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ICollection<RoomOutputDTO>> SearchRoomByNameAsync(string name)
        {
            return await _context.Rooms
                .Where(c => c.Type.Trim().ToLower().Contains(name) || c.Info!.Trim().ToLower().Contains(name))
                .Select(roomDTO => new RoomOutputDTO
                {
                    ID = roomDTO.Id,
                    Type = roomDTO.Type,
                    Price = roomDTO.Price,
                    Taxes = roomDTO.Taxes,
                    Info = roomDTO.Info,
                    Capicity = roomDTO.Capicity,
                    HotelId = roomDTO.HotelId,
                    ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{roomDTO.Image}",
                    Count = roomDTO.Count
                })
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<RoomOutputDTO> CreateRoomAsync(RoomDTO roomDTO)
        {
            Room room = new()
            {
                 Type = roomDTO.Type,
                 Price = roomDTO.Price,
                 Taxes = roomDTO.Taxes,
                 Info = roomDTO.Info,
                 Capicity = roomDTO.Capicity,
                 HotelId = roomDTO.HotelId,
                 Count = roomDTO.Count
            };

            var fileResult = _imageService.SaveImage(roomDTO.ImagePath, _imagesPath);

            if (fileResult.Item1 == 1)
            {
                room.Image = fileResult.Item2;
            }

            await AddAsync(room);

            RoomOutputDTO roomOutputDTO = new()
            {
                ID = room.Id,
                Type = roomDTO.Type,
                Price = roomDTO.Price,
                Taxes = roomDTO.Taxes,
                Info = roomDTO.Info,
                Capicity = roomDTO.Capicity,
                HotelId = roomDTO.HotelId,
                Count = roomDTO.Count,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{room.Image}"
            };

            return roomOutputDTO;
        }
        public async Task<RoomOutputDTO?> UpdateRoom(int roomId,RoomDTO roomDTO)
        {
            var room = await _context.Rooms.SingleOrDefaultAsync(c => c.Id == roomId);
            if (room is null) { return null; }

            string oldImage = room.Image;

            if (roomDTO.ImagePath is not null)
            {
                var fileResult = _imageService.SaveImage(roomDTO.ImagePath, _imagesPath);

                if (fileResult.Item1 == 1)
                {
                    room.Image = fileResult.Item2;
                }
            }

            room.Type = roomDTO.Type;
            room.Price = roomDTO.Price;
            room.Taxes = roomDTO.Taxes;
            room.Capicity = roomDTO.Capicity;
            room.Info = roomDTO.Info;
            room.HotelId = roomDTO.HotelId;

            if (roomDTO.ImagePath is not null)
            {
                _imageService.DeleteImage(oldImage, _imagesPath);
            }

            RoomOutputDTO roomOutputDTO = new()
            {
                ID = room.Id,
                Type = roomDTO.Type,
                Price = roomDTO.Price,
                Taxes = roomDTO.Taxes,
                Info = roomDTO.Info,
                Capicity = roomDTO.Capicity,
                HotelId = roomDTO.HotelId,
                Count = roomDTO.Count,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{room.Image}"
            };

            return roomOutputDTO;

        }
        public bool DeleteRoom(int id)
        {
            var room = _context.Rooms.SingleOrDefault(c => c.Id == id);

            if (room == null)
            {
                return false;
            }

            Delete(room);
            _imageService.DeleteImage(room.Image, _imagesPath);

            return true;
        }
        public bool TypeExist(string type)
        {
            return _context.Rooms.SingleOrDefault(c => c.Type == type) != null;
        }
    }
}
