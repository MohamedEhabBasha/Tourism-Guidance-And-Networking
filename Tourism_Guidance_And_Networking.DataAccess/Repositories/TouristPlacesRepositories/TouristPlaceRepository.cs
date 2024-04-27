

using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.TouristPlacesRepositories
{
    public class TouristPlaceRepository : BaseRepository<TouristPlace>, ITouristPlaceRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly string _imagesPath;
        public TouristPlaceRepository(ApplicationDbContext context,
            IImageService imageService) : base(context)
        {
            _context = context;
            _imageService = imageService;
            _imagesPath = FileSettings.touristplaceImagesPath;
        }

        public async Task<TouristPlaceOutputDTO> CreateTouristPlace(TouristPlaceDTO touristPlaceDTO)
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

            TouristPlaceOutputDTO touristPlaceOutputDTO = new()
            {
                Name = touristPlaceDTO.Name,
                Description = touristPlaceDTO.Description ?? "",
                CategoryId = touristPlaceDTO.CategoryId,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{touristPlace.Image}"
            };

            await AddAsync(touristPlace);

            return touristPlaceOutputDTO; 
        }
        public async Task<ICollection<TouristPlaceOutputDTO>> SearchByName(string name)
        {
            return await _context.Tourists
                .Where(c => c.Name.Trim().ToLower().Contains(name) || c.Description!.Trim().ToLower().Contains(name))
                .Select(t => new TouristPlaceOutputDTO
                {
                    Name = t.Name,
                    Description = t.Description ?? "",
                    CategoryId = t.CategoryId,
                    ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{t.Image}"
                })
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<TouristPlaceOutputDTO?> UpdateTouristPlace(int touristId,TouristPlaceDTO touristPlaceDTO)
        {
            var touristPlace = _context.Tourists.SingleOrDefault(c => c.Id == touristId);

            if (touristPlace is null) { return null; }

            string oldImage = touristPlace.Image;

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

            TouristPlaceOutputDTO touristPlaceOutputDTO = new()
            {
                Name = touristPlaceDTO.Name,
                Description = touristPlaceDTO.Description ?? "",
                CategoryId = touristPlaceDTO.CategoryId,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{touristPlace.Image}"
            };
            return touristPlaceOutputDTO;
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
