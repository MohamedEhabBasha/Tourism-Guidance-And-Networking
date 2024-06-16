
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.DTOs;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;

namespace Tourism_Guidance_And_Networking.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    public class TouristPlaceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TouristPlaceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("touristplaces/{categoryId:int}")]
        public async Task<IActionResult> GetTouristPlacesByCategoryId(int categoryId)
        {
            if (!_unitOfWork.Categories.Exist(categoryId))
                return NotFound();

            var touristPlaces = await _unitOfWork.TouristPlaces.GetTouristPlacesByCategoryIdAsync(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristPlaces);
        }
        [HttpGet("touristplacesByCategoryName")]
        public async Task<IActionResult> GetTouristPlacesByCategoryName(string categoryName)
        {
            if (!_unitOfWork.Categories.ExistByName(categoryName))
                return NotFound();

            var touristPlaces = await _unitOfWork.TouristPlaces.GetTouristPlacesByCategoryName(categoryName);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristPlaces);
        }
        [HttpGet("touristplaces")]
        public async Task<IActionResult> GetAllTouristPlaces()
        {
            var touristPlaces = await _unitOfWork.TouristPlaces.GetTouristPlacesAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristPlaces);
        }
        [HttpGet("touristplacesByName")]
        public async Task<IActionResult> GetTouristPlacesByName(string name)
        {
            var touristPlaces = await _unitOfWork.TouristPlaces.SearchByName(name);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristPlaces);
        }
        [HttpGet("GetTouristPlaceById/{id:int}")]
        public async Task<IActionResult> GetTouristPlaceById(int id)
        {
            if (!_unitOfWork.TouristPlaces.Exist(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var touristPlace = await _unitOfWork.TouristPlaces.GetByIdAsync(id);

            TouristPlaceOutputDTO touristPlaceOutputDTO = new()
            {
                Id = touristPlace!.Id,
                Name = touristPlace.Name,
                Description = touristPlace.Description ?? "",
                CategoryId = touristPlace.CategoryId,
                ImageURL = $"{FileSettings.RootPath}/{FileSettings.touristplaceImagesPath}/{touristPlace.Image}"
            };

            return Ok(touristPlaceOutputDTO);
        }
        [HttpPost("touristplace")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateTouristPlace([FromForm]TouristPlaceDTO touristPlaceDTO)
        {
            if (touristPlaceDTO == null || !_unitOfWork.Categories.Exist(touristPlaceDTO.CategoryId))
                return BadRequest(ModelState);

            /*if (_unitOfWork.Categories.ExistByName(category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("", "Category Already Exist");
                return StatusCode(422, ModelState);
            }*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var touristPlace = await _unitOfWork.TouristPlaces.CreateTouristPlace(touristPlaceDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            TouristPlaceOutputDTO touristPlaceOutputDTO = new()
            {
                Id = touristPlace.Id,
                Name = touristPlace.Name,
                Description = touristPlace.Description ?? "",
                CategoryId = touristPlace.CategoryId,
                ImageURL = $"{FileSettings.RootPath}/{FileSettings.touristplaceImagesPath}/{touristPlace.Image}"
            };
            return StatusCode(201, touristPlaceOutputDTO);
        }

        [HttpPut("{touristplaceId:int}")]
        public IActionResult UpdateTouristPlace([FromRoute] int touristplaceId, [FromForm] TouristPlaceDTO updatedTouristPlace)
        {
            if (updatedTouristPlace == null || !_unitOfWork.Categories.Exist(updatedTouristPlace.CategoryId))
                return BadRequest(ModelState);

            if (!_unitOfWork.TouristPlaces.Exist(touristplaceId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.TouristPlaces.GetById(touristplaceId);

            var output = _unitOfWork.TouristPlaces.UpdateTouristPlace(touristplaceId,updatedTouristPlace);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

           // updatedTouristPlace.Id = touristplace!.Id;

            return Ok(output);
        }

        [HttpDelete("{touristplaceId:int}")]
        public IActionResult DeleteTouristPlace([FromRoute] int touristplaceId)
        {
            if (!_unitOfWork.TouristPlaces.Exist(touristplaceId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _unitOfWork.TouristPlaces.DeleteTouristPlace(touristplaceId!);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }

    }
}
