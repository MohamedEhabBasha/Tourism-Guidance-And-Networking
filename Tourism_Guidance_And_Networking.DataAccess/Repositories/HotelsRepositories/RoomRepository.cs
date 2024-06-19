

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
        private readonly static string _imagesPath = FileSettings.roomImagesPath;
        public RoomRepository(ApplicationDbContext context, IImageService imageService) : base(context) 
        {
            _context = context;
            _imageService = imageService;
        }
        public async Task<RoomOutputDTO> GetRoomById(int id)
        {
            var room = await _context.Rooms.SingleAsync(r => r.Id == id);

            var roomDto = ToRoomOutputDto(room);

            return roomDto;
        }
        public async Task<ICollection<RoomOutputDTO>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(c => c.HotelId == hotelId)
                .AsNoTracking()
                .Select(room => ToRoomOutputDto(room))
                .ToListAsync();
        }
        public async Task<ICollection<RoomOutputDTO>> GetRoomsByTypeAsync(string type, int hotelId)
        {
            return await _context.Rooms
                .Where(c => c.Type.Trim().ToLower().Contains(type) && c.HotelId == hotelId)
                .Select(room => ToRoomOutputDto(room))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ICollection<RoomOutputDTO>> SearchRoomByNameAsync(string name)
        {
            return await _context.Rooms
                .Where(c => c.Type.Trim().ToLower().Contains(name) || c.Info!.Trim().ToLower().Contains(name) || c.Description!.Trim().ToLower().Contains(name))
                .Select(room => ToRoomOutputDto(room))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Room> CreateRoomAsync(RoomDTO roomDTO)
        {
            Room room = new()
            {
                 Type = roomDTO.Type,
                 Price = roomDTO.Price,
                 Taxes = roomDTO.Taxes,
                 Info = roomDTO.Info,
                 Description = roomDTO.Description,
                 Capicity = roomDTO.Capicity,
                 HotelId = roomDTO.HotelId,
                 Count = roomDTO.Count
            };

            var fileResult = _imageService.SaveImage(roomDTO.ImagePath, _imagesPath);

            if (fileResult.Item1 == 1)
            {
                room.Image = fileResult.Item2;
            }

            return await AddAsync(room);
        }
        public RoomOutputDTO UpdateRoom(int roomId,RoomDTO roomDTO)
        {
            var room = _context.Rooms.SingleOrDefault(c => c.Id == roomId);


            string oldImage = room!.Image;

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
            room.Description = roomDTO.Description;
            room.HotelId = roomDTO.HotelId;

            if (roomDTO.ImagePath is not null)
            {
                _imageService.DeleteImage(oldImage, _imagesPath);
            }

            RoomOutputDTO roomOutputDTO = ToRoomOutputDto(room);

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
        public static RoomOutputDTO ToRoomOutputDto(Room room)
        {
            string image;
            if (room.Image.Contains("http"))
                image = room.Image;
            else
                image = $"{FileSettings.RootPath}/{_imagesPath}/{room.Image}";

            RoomOutputDTO roomOutputDTO = new()
            {
                ID = room.Id,
                Type = room.Type,
                Price = room.Price,
                Taxes = room.Taxes,
                Info = room.Info,
                Description = room.Description,
                Capicity = room.Capicity,
                HotelId = room.HotelId,
                Count = room.Count,
                ImageURL = image
            };
            return roomOutputDTO;
        }
    }
}
