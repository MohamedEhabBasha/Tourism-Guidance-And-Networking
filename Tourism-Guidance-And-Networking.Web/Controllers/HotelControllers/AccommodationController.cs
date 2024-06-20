using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;


namespace Tourism_Guidance_And_Networking.Web.Controllers.HotelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    public class AccommodationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccommodationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("accommodations")]
        public async Task<IActionResult> GetAllAccommodationsAsync()
        {
            var accommodations =  await _unitOfWork.Accommodations.GetAllAccommodationsAsync();

            return Ok(accommodations);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAccommodationById(int id)
        {
            var accommodation = await _unitOfWork.Accommodations.GetByIdAsync(id);

            if (accommodation is null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var accommDTO = await _unitOfWork.Accommodations.GetAccommodationByIdAsync(id);
            return Ok(accommDTO);
        }
        [HttpGet("accommodations/{companyId:int}")]
        public async Task<IActionResult> GetAccommodationsByCompanyId(int companyId)
        {
            if (!_unitOfWork.Companies.Exist(companyId))
                return NotFound();

            var accommodations = await _unitOfWork.Accommodations.GetAccommodationsByCompanyIdAsync(companyId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(accommodations);
        }
        [HttpGet("accommodationsByAccommodationType/{companyId:int}")]
        public async Task<IActionResult> GetAccommodationsByAccommodationType(string type, int companyId)
        {
            if (!_unitOfWork.Companies.Exist(companyId))
                return NotFound();
            if (!(await _unitOfWork.Accommodations.TypeExistAsync(type)))
                return NotFound();

            var accommodations = await _unitOfWork.Accommodations.GetAccommodationsByTypeAsync(type,companyId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(accommodations);
        }
        [HttpGet("FilterAccommodationByPrice")]
        public async Task<IActionResult> FilterAccommodationByPrice([FromQuery] double minPrice, [FromQuery] double maxPrice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _unitOfWork.Accommodations.FilterByPrice(minPrice,maxPrice));
        }
        [HttpGet("FilterAccommodationByRate")]
        public async Task<IActionResult> FilterAccommodationByRate([FromQuery] double star)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _unitOfWork.Accommodations.FilterByRate(star));
        }
        [HttpGet("PaginatedAccommodation")]
        public async Task<IActionResult> GetPaginatedAccommodation([FromQuery] int pageNumber)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _unitOfWork.Accommodations.GetPaginatedAccommodationAsync(pageNumber, 200));
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateAccommodation([FromForm] AccommodationDTO accommodationDTO)
        {
            if (accommodationDTO == null || !_unitOfWork.Companies.Exist(accommodationDTO.CompanyId))
                return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Accommodation accommodation = await _unitOfWork.Accommodations.CreateAccommodationAsync(accommodationDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            AccommodationOutputDTO output = new()
            {
                Id = accommodation.Id,
                Name = accommodation.Name,
                Address = accommodation.Address,
                Rating = accommodation.Rating,
                Reviews = accommodation.Reviews,
                Type = accommodation.Type,
                Price = accommodation.Price,
                ImageURL = $"{FileSettings.RootPath}/{FileSettings.accommodationImagesPath}/{accommodation.Image}",
                Taxes = accommodation.Taxes,
                Info = accommodation.Info,
                Capicity = accommodation.Capicity,
                Count = accommodation.Count,
                CompanyId = accommodation.CompanyId,
                Location = accommodation.Location,
                Governorate = accommodation.Governorate,
                Description = accommodation.Description
            };
            return StatusCode(201, output);
        }

        [HttpPost("CreateAccommdationFromCompany")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> CreateAccommodationFromCompany([FromForm] CreateAccomdationDTO accommodationDTO)
        {
            if (accommodationDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            var company = await _unitOfWork.Companies.FindAsync(h => h.ApplicationUserId == applicationUser.Id);

            if (company is null)
                return BadRequest("The Current User is not an Company user");


            AccommodationDTO accommodationDto = new() 
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
                ImagePath = accommodationDTO.ImagePath,
                Count = accommodationDTO.Count,
                CompanyId = company.Id,
                Location = accommodationDTO.Location,
                Governorate = accommodationDTO.Governorate,
                Description = accommodationDTO.Description
            };


            Accommodation accommodation = await _unitOfWork.Accommodations.CreateAccommodationAsync(accommodationDto);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            AccommodationOutputDTO output = new()
            {
                Id = accommodation.Id,
                Name = accommodation.Name,
                Address = accommodation.Address,
                Rating = accommodation.Rating,
                Reviews = accommodation.Reviews,
                Type = accommodation.Type,
                Price = accommodation.Price,
                ImageURL = $"{FileSettings.RootPath}/{FileSettings.accommodationImagesPath}/{accommodation.Image}",
                Taxes = accommodation.Taxes,
                Info = accommodation.Info,
                Capicity = accommodation.Capicity,
                Count = accommodation.Count,
                CompanyId = accommodation.CompanyId,
                Location = accommodation.Location,
                Governorate = accommodation.Governorate,
                Description = accommodation.Description
            };
            return StatusCode(201, output);
        }

        [HttpPost("MakeReview")]
        [Authorize]
        public async Task<IActionResult> MakeReview([FromBody] ReviewDTo reviewDTo)
        {
            var accommdation = _unitOfWork.Accommodations.GetById(reviewDTo.ItemId);

            if (accommdation is null)
                return NotFound($"THERE IS NO ACCOMDATION  WITH ID = {reviewDTo.ItemId}");


            accommdation.Rating = (accommdation.Rating * accommdation.Reviews + reviewDTo.Rating) / (accommdation.Reviews + 1);
            accommdation.Rating = Math.Round(accommdation.Rating, 2);
            accommdation.Reviews++;

            _unitOfWork.Complete();

            return Ok(accommdation);
        }


        [HttpPut("{accommodationId:int}")]
        public IActionResult UpdateAccommodationAsync([FromRoute] int accommodationId, [FromForm] AccommodationDTO accommodationDTO)
        {
            if (accommodationDTO == null || !_unitOfWork.Companies.Exist(accommodationDTO.CompanyId))
                return BadRequest(ModelState);

            if (!_unitOfWork.Accommodations.Exist(accommodationId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _unitOfWork.Accommodations.UpdateAccommodation(accommodationId, accommodationDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            // hotelDTO.Id = hotel!.Id;

            return Ok(output);
        }
        [HttpDelete("{accommodationId:int}")]
        public IActionResult DeleteAccommodation([FromRoute] int accommodationId)
        {
            if (!_unitOfWork.Accommodations.Exist(accommodationId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.Accommodations.DeleteAccommodation(accommodationId!);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }
    }
}
