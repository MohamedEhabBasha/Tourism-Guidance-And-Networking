

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
        private readonly static string _imagesPath = FileSettings.accommodationImagesPath;
        public AccommodationRepository(ApplicationDbContext context, IImageService imageService) : base(context)
        {
            _context = context;
            _imageService = imageService;
        }
        public async Task<ICollection<AccommodationOutputDTO>> GetAllAccommodationsAsync()
        {
            return await _context.Accommodations
              .Select(accommodation => ToAccommodationOutputDto(accommodation))
              .AsNoTracking()
              .ToListAsync();
        }
        public async Task<AccommodationOutputDTO> GetAccommodationByIdAsync(int accomId)
        {
            var accommodationDTO = await _context.Accommodations.SingleOrDefaultAsync(C => C.Id == accomId);

            var accomm = ToAccommodationOutputDto(accommodationDTO!);

            return accomm;
        }
        public async Task<ICollection<AccommodationOutputDTO>> GetAccommodationsByCompanyIdAsync(int companyId)
        {
            return await _context.Accommodations
            .Where(c => c.CompanyId == companyId)
            .Select(accommodation => ToAccommodationOutputDto(accommodation))
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<ICollection<AccommodationOutputDTO>> GetAccommodationsByTypeAsync(string type, int companyId)
        {
            return await _context.Accommodations
                .Where(c => c.Type.Trim().ToLower().Contains(type) && c.CompanyId == companyId)
                .Select(accommodation => ToAccommodationOutputDto(accommodation))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ICollection<AccommodationOutputDTO>> FilterByPrice(double minPrice, double maxPrice)
        {
            return await _context.Accommodations
                .Where(a => ((a.Price >= minPrice && a.Price <= maxPrice)))
                .Select(accommodation => ToAccommodationOutputDto(accommodation))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ICollection<AccommodationOutputDTO>> FilterByRate(double star)
        {
            return await _context.Accommodations
                .Where(a => (a.Rating <= star*2.0 && a.Rating >= (star-1)*2.0))
                .Select(accommodation => ToAccommodationOutputDto(accommodation))
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<PaginationDTO<AccommodationOutputDTO>> GetPaginatedAccommodationAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Accommodations.CountAsync();
            List<AccommodationOutputDTO> items = await _context.Accommodations
                                      .OrderBy(a => a.Id)
                                      .Skip(pageSize * (pageNumber - 1))
                                      .Take(pageSize)
                                      .Select(a => ToAccommodationOutputDto(a))
                                      .ToListAsync();

            return new PaginationDTO<AccommodationOutputDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<Accommodation> CreateAccommodationAsync(AccommodationDTO accommodationDTO)
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
                CompanyId = accommodationDTO.CompanyId,
                Location = accommodationDTO.Location,
                Governorate = accommodationDTO .Governorate,
                Description = accommodationDTO .Description,
                PropertyType = accommodationDTO.PropertyType
            };
            var fileResult = _imageService.SaveImage(accommodationDTO.ImagePath, _imagesPath);

            if (fileResult.Item1 == 1)
            {
                accommodation.Image = fileResult.Item2;
            }

            return await AddAsync(accommodation);
        }
        public AccommodationOutputDTO UpdateAccommodation(int accommodationId, AccommodationDTO accommodationDTO)
        {
            var accommodation = _context.Accommodations.SingleOrDefault(c => c.Id == accommodationId);

            string oldImage = accommodation!.Image;

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
            accommodation.Description = accommodationDTO.Description;
            accommodation.Location = accommodationDTO.Location;
            accommodation.Governorate = accommodationDTO.Governorate;
            accommodation.PropertyType = accommodationDTO.PropertyType;

            if (accommodationDTO.ImagePath is not null)
            {
                _imageService.DeleteImage(oldImage, _imagesPath);
            }

            AccommodationOutputDTO accommodationOutputDTO = ToAccommodationOutputDto(accommodation);

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
        public static AccommodationOutputDTO ToAccommodationOutputDto(Accommodation accommodation)
        {
            string image;
            if (accommodation.Image.Contains("http"))
                image = accommodation.Image;
            else
                image = $"{FileSettings.RootPath}/{_imagesPath}/{accommodation.Image}";

            AccommodationOutputDTO accommodationOutputDTO = new()
            {
                Id = accommodation.Id,
                Name = accommodation.Name,
                Address = accommodation.Address,
                Rating = accommodation.Rating,
                Reviews = accommodation.Reviews,
                Type = accommodation.Type,
                Price = accommodation.Price,
                ImageURL = image,
                Taxes = accommodation.Taxes,
                Info = accommodation.Info,
                Capicity = accommodation.Capicity,
                Count = accommodation.Count,
                CompanyId = accommodation.CompanyId,
                Location = accommodation.Location,
                Governorate = accommodation.Governorate,
                Description = accommodation.Description,
                PropertyType = accommodation.PropertyType
            };

            return accommodationOutputDTO;
        }
    }
}
