namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.HotelsRepositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public HotelRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : base(context) 
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.hotelImagesPath}";
        }
        public async Task<Hotel> GetHotelByNameAsync(string name)
        {
            return await _context.Hotels
                .AsNoTracking()
                .FirstAsync(c => c.Name.Trim().ToLower().Contains(name));
        }

        public async Task<ICollection<Room>> GetRoomsByIdAsync(int hotelId)
        {
            return await _context.Rooms
                .Where(c => c.HotelId == hotelId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<Room>> GetRoomsByTypeAsync(string type,int hotelId)
        {
            return await _context.Rooms
                .Where(c => c.Type.Trim().ToLower().Contains(type) && c.HotelId == hotelId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Hotel> CreateHotelAsync(HotelDTO hotelDTO)
        {
            string coverName = await SaveCover(hotelDTO.ImagePath, _imagesPath);

            Hotel hotel = new() { 
                Name = hotelDTO.Name,
                Address = hotelDTO.Address,
                Rating = hotelDTO.Rating,
                Reviews = hotelDTO.Reviews,
                Image = coverName
            };

            return await AddAsync(hotel);
        }
        public async Task<Hotel?> UpdateHotel(int hotelId,HotelDTO hotelDTO)
        {
            var hotel = _context.Hotels.SingleOrDefault(c => c.Id == hotelId);
            if (hotel is null) { return null; }

            bool hasNewCover = hotelDTO.ImagePath is not null;
            bool equal = Equal(hotel, hotelDTO);
            var oldCover = hotel.Image;

            hotel.Name = hotelDTO.Name;
            hotel.Address = hotelDTO.Address;
            hotel.Rating = hotelDTO.Rating;
            hotel.Reviews = hotelDTO.Reviews;

            if (hasNewCover)
            {
                hotel.Image = await SaveCover(hotelDTO.ImagePath!,_imagesPath);
                equal = oldCover == hotel.Image;
            }
            if (!equal)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }

                return hotel;
            }
            else
            {
                //var cover = Path.Combine(_imagesPath, touristPlace.Image);
                //File.Delete(cover);

                return hotel;
            }
        }
        public bool DeleteHotel(int id)
        {
            var hotel = _context.Hotels.SingleOrDefault(c => c.Id == id);

            if (hotel == null)
            {
                return false;
            }
            var cover = Path.Combine(_imagesPath, hotel.Image);
            File.Delete(cover);

            Delete(hotel);

            return true;
        }

        private static bool Equal(Hotel hotel,HotelDTO hotelDTO)
        {
            if(hotel.Name != hotelDTO.Name || hotel.Address != hotelDTO.Address ||
                hotel.Rating !=  hotelDTO.Rating || hotel.Reviews != hotelDTO.Reviews)
                return false;
            return true;
        }
    }
}
