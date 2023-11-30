using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Guidance_And_Networking.Core.DTOs;
using Tourism_Guidance_And_Networking.Core.Interfaces;
using Tourism_Guidance_And_Networking.Core.Models.TouristPlaces;

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
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }
        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (!ModelState.IsValid || category is null)
                return BadRequest(ModelState);

            return Ok(category);
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
            var touristPlaces = await _unitOfWork.Tours.GetAllAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristPlaces);
        }
        [HttpGet("touristplacesByName")]
        public async Task<IActionResult> GetTouristPlacesByName(string name)
        {
            var touristPlaces = await _unitOfWork.Tours.SearchByName(name.Trim().ToLower());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristPlaces);
        }
        [HttpPost("category")]
        public async Task<IActionResult> CreateCategory(BaseDTO category)
        {
            if (category == null)
                return BadRequest(ModelState);

            if (_unitOfWork.Categories.ExistByName(category.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("", "Category Already Exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Category newCategory = new()
            {
                Name = category.Name
            };

            await _unitOfWork.Categories.AddAsync(newCategory);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            //category.Id = newCategory.Id;
            return StatusCode(201, newCategory);
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

            TouristPlace touristPlace = await _unitOfWork.Tours.CreateTouristPlace(touristPlaceDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }


            return StatusCode(201, touristPlace);
        }

        [HttpPut("{categoryId:int}")]
        public IActionResult UpdateCategory([FromRoute] int categoryId, [FromBody] BaseDTO updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);

            if (!_unitOfWork.Categories.Exist(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _unitOfWork.Categories.GetById(categoryId);

            category!.Name = updatedCategory.Name;

            _unitOfWork.Categories.Update(category);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(category);
        }
        [HttpPut("{touristplaceId}")]
        public IActionResult UpdateTouristPlace([FromRoute] int touristplaceId, [FromBody] TouristPlaceDTO updatedTouristPlace)
        {
            if (updatedTouristPlace == null)
                return BadRequest(ModelState);

            if (!_unitOfWork.Tours.Exist(touristplaceId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var touristplace =  _unitOfWork.Tours.GetById(touristplaceId);

            _unitOfWork.Tours.UpdateTouristPlace(updatedTouristPlace);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            updatedTouristPlace.Id = touristplace!.Id;

            return Ok(updatedTouristPlace);
        }
        [HttpDelete("{categoryId:int}")]
        public IActionResult DeleteCategory([FromRoute] int categoryId)
        {

            if (!_unitOfWork.Categories.Exist(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var touristPlaces = _unitOfWork.Categories.GetTouristPlacesById(categoryId);

            if (touristPlaces.Count > 0)
            {
                ModelState.AddModelError("Forign Key Constrain", "Cannot Delete the category as it refrences by other TouristPlaces");
                return BadRequest(ModelState);
            }

            var categoryDb = _unitOfWork.Categories.GetById(categoryId);

            _unitOfWork.Categories.Delete(categoryDb!);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }

        [HttpDelete("{touristplaceId:int}")]
        public IActionResult DeleteTouristPlace([FromRoute] int touristplaceId)
        {
            if (!_unitOfWork.Categories.Exist(touristplaceId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var touristPlace = _unitOfWork.Tours.GetById(touristplaceId);

            _unitOfWork.Tours.Delete(touristPlace!);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }

    }
}
