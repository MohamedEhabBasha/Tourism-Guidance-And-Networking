﻿
namespace Tourism_Guidance_And_Networking.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

            var touristPlaces = await _unitOfWork.Categories.GetTouristPlacesByIdAsync(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristPlaces);
        }
        [HttpGet("touristplacesByCategoryName")]
        public async Task<IActionResult> GetTouristPlacesByCategoryName(string categoryName)
        {
            if (!_unitOfWork.Categories.ExistByName(categoryName.Trim().ToLower()))
                return NotFound();

            var touristPlaces = await _unitOfWork.Categories.GetTouristPlacesByName(categoryName);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristPlaces);
        }
        [HttpGet("touristplaces")]
        public async Task<IActionResult> GetAllTouristPlaces()
        {
            var touristPlaces = await _unitOfWork.TouristPlaces.GetAllAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristPlaces);
        }
        [HttpGet("touristplacesByName")]
        public async Task<IActionResult> GetTouristPlacesByName(string name)
        {
            var touristPlaces = await _unitOfWork.TouristPlaces.SearchByName(name.Trim().ToLower());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristPlaces);
        }

        [HttpPost("touristplace")]
        public async Task<IActionResult> CreateTouristPlace(TouristPlaceDTO touristPlaceDTO)
        {
            if (touristPlaceDTO == null)
                return BadRequest(ModelState);

            /*if (_unitOfWork.Categories.ExistByName(category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("", "Category Already Exist");
                return StatusCode(422, ModelState);
            }*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TouristPlace touristPlace = await _unitOfWork.TouristPlaces.CreateTouristPlace(touristPlaceDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }


            return StatusCode(201, touristPlace);
        }

        [HttpPut("{touristplaceId}")]
        public IActionResult UpdateTouristPlace([FromRoute] int touristplaceId, [FromBody] TouristPlaceDTO updatedTouristPlace)
        {
            if (updatedTouristPlace == null)
                return BadRequest(ModelState);

            if (!_unitOfWork.TouristPlaces.Exist(touristplaceId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var touristplace =  _unitOfWork.TouristPlaces.GetById(touristplaceId);

            _unitOfWork.TouristPlaces.UpdateTouristPlace(updatedTouristPlace);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            updatedTouristPlace.Id = touristplace!.Id;

            return Ok(updatedTouristPlace);
        }

        [HttpDelete("{touristplaceId:int}")]
        public IActionResult DeleteTouristPlace([FromRoute] int touristplaceId)
        {
            if (!_unitOfWork.Categories.Exist(touristplaceId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var touristPlace = _unitOfWork.TouristPlaces.GetById(touristplaceId);

            _unitOfWork.TouristPlaces.Delete(touristPlace!);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }

    }
}
