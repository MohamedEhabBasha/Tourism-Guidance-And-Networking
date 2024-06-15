using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.Company)]
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
