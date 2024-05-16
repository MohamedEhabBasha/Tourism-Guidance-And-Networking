
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
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("companies")]
        public async Task<IActionResult> GetAllCompaniesAsync()
        {
            var companies = await _unitOfWork.Companies.GetAllAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(companies);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(id);

            if (company == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(company);
        }
        [HttpGet("companyByName")]
        public async Task<IActionResult> GetCompanyByName(string name)
        {
            var company = await _unitOfWork.Companies.GetCompanyByNameAsync(name);

            if (company is null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(company);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateCompanyAsync([FromForm] CompanyDTO companyDTO)
        {
            if (companyDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            CompanyOutputDTO company = await _unitOfWork.Companies.CreateCompanyAsync(companyDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return StatusCode(201, company);
        }
        [HttpPut("{companyId:int}")]
        public async Task<IActionResult> UpdateCompanyAsync([FromRoute] int companyId, [FromForm] CompanyDTO companyDTO)
        {
            if (companyDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_unitOfWork.Companies.Exist(companyId))
                return NotFound();

            var company = _unitOfWork.Companies.GetById(companyId);

            await _unitOfWork.Companies.UpdateCompany(companyId, companyDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            // hotelDTO.Id = hotel!.Id;

            return Ok(company);
        }
        [HttpDelete("{companyId:int}")]
        public IActionResult DeleteCompany([FromRoute] int companyId)
        {
            if (!_unitOfWork.Companies.Exist(companyId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.Companies.DeleteCompany(companyId);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }
    }
}
