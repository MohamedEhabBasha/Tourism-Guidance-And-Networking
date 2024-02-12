

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.TouristPlacesRepositories
{
    public class TouristPlaceRepository : BaseRepository<TouristPlace>, ITouristPlaceRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public TouristPlaceRepository(ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }

        public async Task<TouristPlace> CreateTouristPlace(TouristPlaceDTO touristPlaceDTO)
        {
            string coverName = await SaveCover(touristPlaceDTO.ImagePath,_imagesPath);

            TouristPlace touristPlace = new()
            {
                Name = touristPlaceDTO.Name,
                Description = touristPlaceDTO.Description,
                CategoryId = touristPlaceDTO.CategoryId,
                Image = coverName
            };

            return await AddAsync(touristPlace);
        }
        public async Task<ICollection<TouristPlace>> SearchByName(string name)
        {
            return await _context.Tourists
                .Where(c => c.Name.Trim().ToLower().Contains(name) || c.Description!.Trim().ToLower().Contains(name))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<TouristPlace?> UpdateTouristPlace(int touristId,TouristPlaceDTO touristPlaceDTO)
        {
            var touristPlace = _context.Tourists.SingleOrDefault(c => c.Id == touristId);

            if (touristPlace is null) { return null; }

            bool hasNewCover = touristPlaceDTO.ImagePath is not null;
            bool equal = Equal(touristPlace, touristPlaceDTO);
            var oldCover = touristPlace.Image;

            touristPlace.Name = touristPlaceDTO.Name;
            touristPlace.Description = touristPlaceDTO.Description;
            touristPlace.CategoryId = touristPlaceDTO.CategoryId;

            if (hasNewCover)
            {
                touristPlace.Image = await SaveCover(touristPlaceDTO.ImagePath!,_imagesPath);
                equal = oldCover == touristPlace.Image;
            }
            if (!equal)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }

                return touristPlace;
            }
            else
            {
                //var cover = Path.Combine(_imagesPath, touristPlace.Image);
                //File.Delete(cover);

                return touristPlace;
            }
        }
        public bool DeleteTouristPlace(int id)
        {
            var touristPlace = _context.Tourists.SingleOrDefault(c => c.Id == id);

            if(touristPlace == null)
            {
                return false;
            }
            var cover = Path.Combine(_imagesPath, touristPlace.Image);
            File.Delete(cover);

            Delete(touristPlace);

            return true;
        }

        private static bool Equal(TouristPlace touristPlace, TouristPlaceDTO touristPlaceDTO)
        {
            if (touristPlace.Name != touristPlaceDTO.Name               ||
                touristPlace.Description != touristPlaceDTO.Description ||
                touristPlace.CategoryId != touristPlaceDTO.CategoryId
                )
                return false;

            return true;
        }
    }
}
