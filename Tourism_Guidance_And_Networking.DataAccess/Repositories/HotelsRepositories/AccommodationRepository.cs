

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface;

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
        public Task<ICollection<Accommodation>> GetAccommodationByCompanyIdAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Accommodation>> GetAccommodationByTypeAsync(string type, int companyId)
        {
            throw new NotImplementedException();
        }
        public Task<Accommodation> CreateAccommodationAsync(AccommodationDTO accommodationDTO)
        {
            throw new NotImplementedException();
        }
        public Task<Accommodation?> UpdateAccommodation(int accommodationId, AccommodationDTO accommodationDTO)
        {
            throw new NotImplementedException();
        }
        public bool DeleteAccommodation(int id)
        {
            throw new NotImplementedException();
        }
    }
}
