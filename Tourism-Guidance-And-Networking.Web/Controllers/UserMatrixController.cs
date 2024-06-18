namespace Tourism_Guidance_And_Networking.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserMatrixController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public UserMatrixController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [HttpGet("GetAllUsersMatrix")]
    public async Task<IActionResult> GetAllUserMatrices() 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await _unitOfWork.UserMatrix.GetAllAsync());
    }
    [HttpGet("SeedingUserMatrix")]
    public async Task<IActionResult> SeedingUserMatrix()
    {
        var once = await _unitOfWork.UserMatrix.FindAsync(u => u.UserID == "60728638-96e8-4576-ac14-da785002ee04");

        if (once is not null)
            return BadRequest("Data Already Exists");

        List<UserMatrix> list = _unitOfWork.UserMatrix.CreateAllUserMatrices();

        await _unitOfWork.UserMatrix.AddRangeAsync(list);

        if (!(_unitOfWork.Complete() > 0))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Added Successfully");
    }
}
