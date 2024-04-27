

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.HotelsRepositories
{
    public class AccommodationRepository : BaseRepository<Accommodation>, IAccommodationRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly string _imagesPath;
        public AccommodationRepository(ApplicationDbContext context, IImageService imageService) : base(context)
        {
            _context = context;
            _imageService = imageService;
            _imagesPath = FileSettings.accommodationImagesPath; 
        }
        public async Task<ICollection<AccommodationOutputDTO>> GetAccommodationsByCompanyIdAsync(int companyId)
        {
            return await _context.Accommodations
            .Where(c => c.CompanyId == companyId)
            .Select(accommodationDTO => new AccommodationOutputDTO
            {
                Name = accommodationDTO.Name,
                Address = accommodationDTO.Address,
                Rating = accommodationDTO.Rating,
                Reviews = accommodationDTO.Reviews,
                Type = accommodationDTO.Type,
                Price = accommodationDTO.Price,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{accommodationDTO.Image}",
                Taxes = accommodationDTO.Taxes,
                Info = accommodationDTO.Info,
                Capicity = accommodationDTO.Capicity,
                Count = accommodationDTO.Count,
                CompanyId = accommodationDTO.CompanyId
            })
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<ICollection<AccommodationOutputDTO>> GetAccommodationsByTypeAsync(string type, int companyId)
        {
            return await _context.Accommodations
                .Where(c => c.Type.Trim().ToLower().Contains(type) && c.CompanyId == companyId)
                .Select(accommodationDTO => new AccommodationOutputDTO
                {
                    Name = accommodationDTO.Name,
                    Address = accommodationDTO.Address,
                    Rating = accommodationDTO.Rating,
                    Reviews = accommodationDTO.Reviews,
                    Type = accommodationDTO.Type,
                    Price = accommodationDTO.Price,
                    ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{accommodationDTO.Image}",
                    Taxes = accommodationDTO.Taxes,
                    Info = accommodationDTO.Info,
                    Capicity = accommodationDTO.Capicity,
                    Count = accommodationDTO.Count,
                    CompanyId = accommodationDTO.CompanyId
                })
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<AccommodationOutputDTO> CreateAccommodationAsync(AccommodationDTO accommodationDTO)
        {
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
                Capicity = accommodationDTO.Capicity,
                Count = accommodationDTO.Count,
                CompanyId = accommodationDTO.CompanyId
            };
            var fileResult = _imageService.SaveImage(accommodationDTO.ImagePath, _imagesPath);

            if (fileResult.Item1 == 1)
            {
                accommodation.Image = fileResult.Item2;
            }
            AccommodationOutputDTO accommodationOutputDTO = new()
            {
                Name = accommodationDTO.Name,
                Address = accommodationDTO.Address,
                Rating = accommodationDTO.Rating,
                Reviews = accommodationDTO.Reviews,
                Type = accommodationDTO.Type,
                Price = accommodationDTO.Price,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{fileResult.Item2}",
                Taxes = accommodationDTO.Taxes,
                Info = accommodationDTO.Info,
                Capicity = accommodationDTO.Capicity,
                Count = accommodationDTO.Count,
                CompanyId = accommodationDTO.CompanyId
            };
            await AddAsync(accommodation);
            return accommodationOutputDTO;
        }
        public async Task<AccommodationOutputDTO?> UpdateAccommodation(int accommodationId, AccommodationDTO accommodationDTO)
        {
            var accommodation = await _context.Accommodations.SingleOrDefaultAsync(c => c.Id == accommodationId);
            if (accommodation is null) { return null; }

            string oldImage = accommodation.Image;

            if (accommodationDTO.ImagePath is not null)
            {
                var fileResult = _imageService.SaveImage(accommodationDTO.ImagePath, _imagesPath);

                if (fileResult.Item1 == 1)
                {
                    accommodation.Image = fileResult.Item2;
                }
            }

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

            if (accommodationDTO.ImagePath is not null)
            {
                _imageService.DeleteImage(oldImage, _imagesPath);
            }

            AccommodationOutputDTO accommodationOutputDTO = new()
            {
                Name = accommodationDTO.Name,
                Address = accommodationDTO.Address,
                Rating = accommodationDTO.Rating,
                Reviews = accommodationDTO.Reviews,
                Type = accommodationDTO.Type,
                Price = accommodationDTO.Price,
                ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{accommodation.Image}",
                Taxes = accommodationDTO.Taxes,
                Info = accommodationDTO.Info,
                Capicity = accommodationDTO.Capicity,
                Count = accommodationDTO.Count,
                CompanyId = accommodationDTO.CompanyId
            };

            return accommodationOutputDTO;
        }
        public bool DeleteAccommodation(int id)
        {
            var accommodation = _context.Accommodations.SingleOrDefault(c => c.Id == id);

            if (accommodation == null)
            {
                return false;
            }

            Delete(accommodation);
            _imageService.DeleteImage(accommodation.Image, _imagesPath);

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
