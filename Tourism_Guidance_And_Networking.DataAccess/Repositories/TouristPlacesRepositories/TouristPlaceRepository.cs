

using Tourism_Guidance_And_Networking.Core.DTOs;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.TouristPlacesRepositories
{
    public class TouristPlaceRepository : BaseRepository<TouristPlace>, ITouristPlaceRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly static string _imagesPath = FileSettings.touristplaceImagesPath;
        public TouristPlaceRepository(ApplicationDbContext context,
            IImageService imageService) : base(context)
        {
            _context = context;
            _imageService = imageService;
        }
        public async Task<ICollection<TouristPlaceOutputDTO>> GetTouristPlacesAsync()
        {
            return await _context.Tourists
                .Select(t => ToTouristPlaceOutputDto(t))
                .ToListAsync();
        }
        public async Task<ICollection<TouristPlaceOutputDTO>> GetTouristPlacesByCategoryIdAsync(int categoryId)
        {
            return await _context.Tourists
                    .Where(c => c.CategoryId == categoryId)
                    .Select(t => ToTouristPlaceOutputDto(t))
                    .AsNoTracking()
                    .ToListAsync();
        }
        public async Task<ICollection<TouristPlaceOutputDTO>> GetTouristPlacesByCategoryName(string name)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.Name.Trim().ToLower().Contains(name));

            return await _context.Tourists
                .Where(t => t.CategoryId == category.Id)
                .Select(t => ToTouristPlaceOutputDto(t))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ICollection<TouristPlaceOutputDTO>> SearchByName(string name)
        {
            return await _context.Tourists
                .Where(c => c.Name.Trim().ToLower().Contains(name) || c.Description!.Trim().ToLower().Contains(name))
                .Select(t => ToTouristPlaceOutputDto(t))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<TouristPlace> CreateTouristPlace(TouristPlaceDTO touristPlaceDTO)
        {
            TouristPlace touristPlace = new()
            {
                Name = touristPlaceDTO.Name,
                Description = touristPlaceDTO.Description,
                CategoryId = touristPlaceDTO.CategoryId
            };
            var fileResult = _imageService.SaveImage(touristPlaceDTO.ImagePath, _imagesPath);

            if (fileResult.Item1 == 1)
            {
                touristPlace.Image = fileResult.Item2;
            }

            return await AddAsync(touristPlace);
        }

        public TouristPlaceOutputDTO UpdateTouristPlace(int touristId,TouristPlaceDTO touristPlaceDTO)
        {
            var touristPlace = _context.Tourists.SingleOrDefault(c => c.Id == touristId);

            string oldImage = touristPlace!.Image;

            if (touristPlaceDTO.ImagePath is not null)
            {
                var fileResult = _imageService.SaveImage(touristPlaceDTO.ImagePath, _imagesPath);

                if (fileResult.Item1 == 1)
                {
                    touristPlace.Image = fileResult.Item2;
                }
            }

            touristPlace.Name = touristPlaceDTO.Name;
            touristPlace.Description = touristPlaceDTO.Description;
            touristPlace.CategoryId = touristPlaceDTO.CategoryId;

            if (touristPlaceDTO.ImagePath is not null)
            {
                _imageService.DeleteImage(oldImage, _imagesPath);
            }

            TouristPlaceOutputDTO touristPlaceOutputDTO = ToTouristPlaceOutputDto(touristPlace);
            return touristPlaceOutputDTO;
        }
        public bool DeleteTouristPlace(int id)
        {
            var touristPlace = _context.Tourists.SingleOrDefault(c => c.Id == id);

            Delete(touristPlace!);
            _imageService.DeleteImage(touristPlace!.Image, _imagesPath);

            return true;
        }
        public static TouristPlaceOutputDTO ToTouristPlaceOutputDto(TouristPlace touristPlace)
        {
            string image;
            if (touristPlace.Image.Contains("http"))
                image = touristPlace.Image;
            else
                image = $"{FileSettings.RootPath}/{_imagesPath}/{touristPlace.Image}";

            TouristPlaceOutputDTO touristPlaceOutputDTO = new()
            {
                Id = touristPlace.Id,
                Name = touristPlace.Name,
                Description = touristPlace.Description ?? "",
                CategoryId = touristPlace.CategoryId,
                ImageURL = image
            };

            return touristPlaceOutputDTO;
        }
    }
}
