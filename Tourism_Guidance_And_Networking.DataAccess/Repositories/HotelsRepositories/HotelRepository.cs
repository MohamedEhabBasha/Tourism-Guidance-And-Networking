
namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.HotelsRepositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly string _imagesPath;
        public HotelRepository(ApplicationDbContext context, IImageService imageService) : base(context) 
        {
            _context = context;
            _imageService = imageService;
            _imagesPath = FileSettings.hotelImagesPath;
        }
        public async Task<ICollection<HotelOutputDTO>> GetAllHotels()
        {
            return await _context.Hotels.Select(h => new HotelOutputDTO { 
            ID = h.Id,
            Address = h.Address,
            Name = h.Name,
            Rating = h.Rating,
            Reviews = h.Reviews,
            ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{h.Image}"
            }).ToListAsync(); 
        }
        public async Task<HotelOutputDTO> GetHotelByIdAsync(int id)
        {
            var h = await _context.Hotels.SingleAsync(h => h.Id == id);

            var hotelOutputDto = new HotelOutputDTO
            {
                ID = h.Id,
                Address = h.Address,
                Name = h.Name,
                Rating = h.Rating,
                Reviews = h.Reviews,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{h.Image}"
            };

            return hotelOutputDto;
        }
        public async Task<HotelOutputDTO> GetHotelByNameAsync(string name)
        {
            return await _context.Hotels
                .Select(h => new HotelOutputDTO
                {
                    ID = h.Id,
                    Address = h.Address,
                    Name = h.Name,
                    Rating = h.Rating,
                    Reviews = h.Reviews,
                    ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{h.Image}"
                })
                .AsNoTracking()
                .FirstAsync(c => c.Name.Trim().ToLower().Contains(name));
        }
        public async Task<HotelOutputDTO> CreateHotelAsync(HotelDTO hotelDTO)
        {
            Hotel hotel = new() {
                Name = hotelDTO.Name,
                Address = hotelDTO.Address,
                Rating = hotelDTO.Rating,
                Reviews = hotelDTO.Reviews,
                ApplicationUserId = hotelDTO.ApplicationUserId
            };

            var fileResult = _imageService.SaveImage(hotelDTO.ImagePath, _imagesPath);

            if (fileResult.Item1 == 1)
            {
                hotel.Image = fileResult.Item2;
            }

            await AddAsync(hotel);

            HotelOutputDTO hoteldTo = new()
            {
                ID = hotel.Id,
                Address = hotelDTO.Address,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{fileResult.Item2}",
                Rating = hotelDTO.Rating,
                Name = hotelDTO.Name,
                Reviews = hotelDTO.Reviews
            };

            return hoteldTo;
        }
        public async Task<HotelOutputDTO?> UpdateHotel(int hotelId,HotelDTO hotelDTO)
        {
            var hotel = await _context.Hotels.SingleOrDefaultAsync(c => c.Id == hotelId);
            if (hotel is null) { return null; }

            string oldImage = hotel.Image;

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

            if (hotelDTO.ImagePath is not null)
            {
                _imageService.DeleteImage(oldImage, _imagesPath);
            }

            HotelOutputDTO hoteldTo = new()
            {
                ID = hotel.Id,
                Address = hotelDTO.Address,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{hotel.Image}",
                Rating = hotelDTO.Rating,
                Name = hotelDTO.Name,
                Reviews = hotelDTO.Reviews
            };

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
    }
}
