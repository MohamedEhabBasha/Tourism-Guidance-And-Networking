
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.HotelsRepositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly static string _imagesPath = FileSettings.hotelImagesPath;
        public HotelRepository(ApplicationDbContext context, IImageService imageService) : base(context) 
        {
            _context = context;
            _imageService = imageService;
        }
        public async Task<ICollection<HotelOutputDTO>> GetAllHotels()
        {
            return await _context.Hotels.Select(h => ToHotelOutputDto(h)).ToListAsync(); 
        }
        public async Task<HotelOutputDTO> GetHotelByIdAsync(int id)
        {
            var h = await _context.Hotels.SingleAsync(h => h.Id == id);

            var hotelOutputDto = ToHotelOutputDto(h);

            return hotelOutputDto;
        }
        public async Task<ICollection<HotelOutputDTO>> GetHotelByNameAsync(string name)
        {
            return await _context.Hotels
                .Where(h => h.Name.Trim().ToLower().Contains(name))
                .Select(h => ToHotelOutputDto(h))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Hotel> CreateHotelAsync(HotelDTO hotelDTO)
        {
            Hotel hotel = new() {
                Name = hotelDTO.Name,
                Address = hotelDTO.Address,
                Rating = hotelDTO.Rating,
                Reviews = hotelDTO.Reviews,
                ApplicationUserId = hotelDTO.ApplicationUserId,
                Location = hotelDTO.Location,
                Governorate = hotelDTO.Governorate
            };

            var fileResult = _imageService.SaveImage(hotelDTO.ImagePath, _imagesPath);

            if (fileResult.Item1 == 1)
            {
                hotel.Image = fileResult.Item2;
            }

            return await AddAsync(hotel);
        }
        public HotelOutputDTO UpdateHotel(int hotelId,HotelDTO hotelDTO)
        {
            var hotel = _context.Hotels.SingleOrDefault(c => c.Id == hotelId);

            string oldImage = hotel!.Image;

            if (hotelDTO.ImagePath is not null)
            {
                var fileResult = _imageService.SaveImage(hotelDTO.ImagePath, _imagesPath);

                if (fileResult.Item1 == 1)
                {
                    hotel.Image = fileResult.Item2;
                }
            }

            hotel.Name = hotelDTO.Name;
            hotel.Address = hotelDTO.Address;
            hotel.Rating = hotelDTO.Rating;
            hotel.Reviews = hotelDTO.Reviews;
            hotel.Governorate = hotelDTO.Governorate;
            hotel.Location = hotelDTO.Location;

            if (hotelDTO.ImagePath is not null)
            {
                _imageService.DeleteImage(oldImage, _imagesPath);
            }

            HotelOutputDTO hoteldTo = ToHotelOutputDto(hotel);

            return hoteldTo;
        }
        public bool DeleteHotel(int id)
        {
            var hotel = _context.Hotels.SingleOrDefault(c => c.Id == id);

            if (hotel == null)
            {
                return false;
            }

            Delete(hotel);
            _imageService.DeleteImage(hotel.Image, _imagesPath);

            return true;
        }
        public static HotelOutputDTO ToHotelOutputDto(Hotel hotel)
        {
            HotelOutputDTO hoteldTo = new()
            {
                ID = hotel.Id,
                Address = hotel.Address,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{hotel.Image}",
                Rating = hotel.Rating,
                Name = hotel.Name,
                Reviews = hotel.Reviews,
                Location = hotel.Location,
                Governorate = hotel.Governorate,
                //ApplicationUserId = hotel.ApplicationUserId
            };

            return hoteldTo;
        }
    }
}
