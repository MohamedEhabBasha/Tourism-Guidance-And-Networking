
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Authentication;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;
using Tourism_Guidance_And_Networking.Web.Services;

namespace Tourism_Guidance_And_Networking.Web.Controllers.HotelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public CompanyController(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;

        }
        [HttpGet("companies")]
        public async Task<IActionResult> GetAllCompaniesAsync()
        {
            var companies = await _unitOfWork.Companies.GetAllCompaniesAsync();

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

            var companyDto = await _unitOfWork.Companies.GetCompanyById(id);

            return Ok(companyDto);
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

        [HttpGet("GetCompanyUser")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> GetCompanylUser()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            var company = await _unitOfWork.Companies.FindAsync(h => h.ApplicationUserId == applicationUser.Id);

            if (company is null)
                return BadRequest("The Current User is not an company user");

            var Accomdations = await _unitOfWork.Accommodations.FindAllAsync(a => a.CompanyId == company.Id);

            CompanyUserDTO companyUser = new()
            {
                User = applicationUser,
                Company = company,
                Accommodations = Accomdations
            };
            return Ok(companyUser);
        }
        [HttpGet("FilterCompanyByRate")]
        public async Task<IActionResult> FilterCompanyByRate([FromQuery] double rate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _unitOfWork.Companies.FilterByRate(rate));
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateCompanyAsync([FromForm] CreateCompanyDTO companyDTO)
        {
            if (companyDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            RegisterModel registerModel = new RegisterModel();
            registerModel.FirstName = companyDTO.Name;
            registerModel.LastName = companyDTO.Name;
            registerModel.Username = companyDTO.Username;
            registerModel.Address = companyDTO.Address;
            registerModel.PhoneNumber = companyDTO.PhoneNumber;
            registerModel.Email = companyDTO.Email;
            registerModel.Password = companyDTO.Password;
            registerModel.ConfirmPassword = companyDTO.ConfirmPassword;

            var result = await _authService.RegisterAsync(registerModel,Roles.Company);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookies(result.RefreshToken, result.RefrshTokenExpiration);

            CompanyDTO mycompanyDTO = new()
            {
                Name = companyDTO.Name,
                Address = companyDTO.Address,
                Rating = companyDTO.Rating,
                Reviews = companyDTO.Reviews,
                ImagePath = companyDTO.ImagePath,
                ApplicationUserId = result.UserId
            };
            Company company = await _unitOfWork.Companies.CreateCompanyAsync(mycompanyDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            CreateCompanyResultDTO createCompanyResultDTO = new()
            {
                CompanyId = company.Id,
                Message = result.Message,
                IsAuthenticated = result.IsAuthenticated,
                Username = result.Username,
                Email = result.Email,
                UserId = result.UserId,
                Roles = result.Roles,
                Token = result.Token,
                RefreshToken = result.RefreshToken,
                RefrshTokenExpiration = result.RefrshTokenExpiration

            };

            return StatusCode(201, createCompanyResultDTO);
        }

        [HttpPost("MakeReview")]
        [Authorize]
        public async Task<IActionResult> MakeReview([FromBody] ReviewDTo reviewDTo)
        {
            var company = _unitOfWork.Companies.GetById(reviewDTo.ItemId);

            if (company is null)
                return NotFound($"THERE IS NO COMPANY WITH ID = {reviewDTo.ItemId}");


            company.Rating = (company.Rating * company.Reviews + reviewDTo.Rating) / (company.Reviews + 1);
            company.Rating = Math.Round(company.Rating, 2);
            company.Reviews++;

            _unitOfWork.Complete();

            return Ok(company);
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
        private void SetRefreshTokenInCookies(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(), // important as time in postman diffrent from local
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None

            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
