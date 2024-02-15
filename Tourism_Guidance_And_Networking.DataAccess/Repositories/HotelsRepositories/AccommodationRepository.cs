

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.HotelsRepositories
{
    public class AccommodationRepository : BaseRepository<Accommodation>, IAccommodationRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public AccommodationRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.accommodationImagesPath}";
        }
        public async Task<ICollection<Accommodation>> GetAccommodationsByCompanyIdAsync(int companyId)
        {
            return await _context.Accommodations
            .Where(c => c.CompanyId == companyId)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<ICollection<Accommodation>> GetAccommodationsByTypeAsync(string type, int companyId)
        {
            return await _context.Accommodations
                .Where(c => c.Type.Trim().ToLower().Contains(type) && c.CompanyId == companyId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Accommodation> CreateAccommodationAsync(AccommodationDTO accommodationDTO)
        {
            string coverName = await SaveCover(accommodationDTO.ImagePath, _imagesPath);

            Accommodation accommodation = new()
            {
                Name = accommodationDTO.Name,
                Address = accommodationDTO.Address,
                Rating = accommodationDTO.Rating,
                Reviews = accommodationDTO.Reviews,
                Type = accommodationDTO.Type,
                Price = accommodationDTO.Price,
                Taxes = accommodationDTO.Taxes,
                Info = accommodationDTO.Info,
                Image = coverName,
                Capicity = accommodationDTO.Capicity,
                Count = accommodationDTO.Count,
                CompanyId = accommodationDTO.CompanyId
            };

            return await AddAsync(accommodation);
        }
        public async Task<Accommodation?> UpdateAccommodation(int accommodationId, AccommodationDTO accommodationDTO)
        {
            var accommodation = _context.Accommodations.SingleOrDefault(c => c.Id == accommodationId);
            if (accommodation is null) { return null; }

            bool hasNewCover = accommodationDTO.ImagePath is not null;
            bool equal = Equal(accommodation, accommodationDTO);
            var oldCover = accommodation.Image;

            accommodation.Name = accommodationDTO.Name;
            accommodation.Address = accommodationDTO.Address;
            accommodation.Rating = accommodationDTO.Rating;
            accommodation.Reviews = accommodationDTO.Reviews;
            accommodation.Type = accommodationDTO.Type;
            accommodation.Price = accommodationDTO.Price;
            accommodation.Taxes = accommodationDTO.Taxes;
            accommodation.Info = accommodationDTO.Info;
            accommodation.Capicity = accommodationDTO.Capicity;
            accommodation.Count = accommodationDTO.Count;
            accommodation.CompanyId = accommodationDTO.CompanyId;

            if (hasNewCover)
            {
                accommodation.Image = await SaveCover(accommodationDTO.ImagePath!, _imagesPath);
                equal = oldCover == accommodation.Image;
            }
            if (!equal)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }

                return accommodation;
            }
            else
            {
                //var cover = Path.Combine(_imagesPath, touristPlace.Image);
                //File.Delete(cover);

                return accommodation;
            }
        }
        public bool DeleteAccommodation(int id)
        {
            var accommodation = _context.Accommodations.SingleOrDefault(c => c.Id == id);

            if (accommodation == null)
            {
                return false;
            }
            var cover = Path.Combine(_imagesPath, accommodation.Image);
            File.Delete(cover);

            Delete(accommodation);

            return true;
        }
        private static bool Equal(Accommodation accommodation, AccommodationDTO accommodationDTO)
        {
            if (accommodation.Name != accommodationDTO.Name || accommodation.Address != accommodationDTO.Address ||
                accommodation.Rating != accommodationDTO.Rating || accommodation.Reviews != accommodationDTO.Reviews || accommodation.Type == accommodationDTO.Type ||
                accommodation.Price == accommodationDTO.Price || accommodation.Taxes == accommodationDTO.Taxes || accommodation.Info == accommodationDTO.Info ||
                accommodation.Capicity == accommodationDTO.Capicity || accommodation.Count == accommodationDTO.Count || accommodation.CompanyId == accommodationDTO.CompanyId)
                return false;
            return true;
        }

        public async Task<bool> TypeExistAsync(string type)
        {
            var accommodations = await _context.Accommodations.FirstOrDefaultAsync(ac => ac.Type == type);
            if(accommodations == null) return false;
            return true;
        }
    }
}
