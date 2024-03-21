
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Tourism_Guidance_And_Networking.Core.Consts;

namespace Tourism_Guidance_And_Networking.Web.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (!ModelState.IsValid || category is null)
                return BadRequest(ModelState);

            return Ok(category);
        }
        [HttpGet("categoryName")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var category = await _unitOfWork.Categories.GetCategoryByNameAsync(name);

            if (!ModelState.IsValid || category is null)
                return BadRequest(ModelState);

            return Ok(category);
        }
        [HttpPost("category")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateCategory(CategoryDTO category)
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
        [HttpPut("{categoryId:int}")]
        public IActionResult UpdateCategory([FromRoute] int categoryId, [FromBody] CategoryDTO updatedCategory)
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

    }
}
