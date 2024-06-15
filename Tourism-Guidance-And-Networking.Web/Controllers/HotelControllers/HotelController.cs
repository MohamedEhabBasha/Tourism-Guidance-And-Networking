
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.DTOs;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Authentication;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;
using Tourism_Guidance_And_Networking.Web.Services;

namespace Tourism_Guidance_And_Networking.Web.Controllers.HotelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    public class HotelController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public HotelController(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        [HttpGet("hotels")]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _unitOfWork.Hotels.GetAllHotels();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(hotels);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);
            if(hotel is null)
                return NotFound("Hotel does not exist");
            var hotelDto = await _unitOfWork.Hotels.GetHotelByIdAsync(id);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(hotelDto);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Hotel)]
        public async Task<IActionResult> GetHotelUser()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            var hotel = await _unitOfWork.Hotels.FindAsync(h => h.ApplicationUserId == applicationUser.Id);

            if (hotel is null)
                return BadRequest("The Current User is not an hotel user");

            var rooms = await _unitOfWork.Rooms.FindAllAsync(r=> r.HotelId == hotel.Id);

            HoteluserDTO hotelUser= new()
            {
                User = applicationUser,
                Hotel = hotel,
                Rooms = rooms
            };
            return Ok(hotelUser);
        }


        [HttpGet("hotelByName")]
        public async Task<IActionResult> GetHotelByName(string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotelDto = await _unitOfWork.Hotels.GetHotelByNameAsync(name);
            return Ok(hotelDto);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateHotel([FromForm] CreateHotelDto hotelDTO)
        {
            if (hotelDTO is null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            RegisterModel registerModel = new RegisterModel();
            registerModel.FirstName = hotelDTO.Name;
            registerModel.LastName = hotelDTO.Name;
            registerModel.Username = hotelDTO.Username;
            registerModel.Address = hotelDTO.Address;
            registerModel.PhoneNumber = hotelDTO.PhoneNumber;
            registerModel.Email = hotelDTO.Email;
            registerModel.Password = hotelDTO.Password;
            registerModel.ConfirmPassword = hotelDTO.ConfirmPassword;

            var result = await _authService.RegisterAsync(registerModel, Roles.Hotel);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookies(result.RefreshToken, result.RefrshTokenExpiration);
            HotelDTO myHotelDTO = new()
            {
                Name = hotelDTO.Name,
                Address = hotelDTO.Address,
                Rating = hotelDTO.Rating,
                Reviews = hotelDTO.Reviews,
                ImagePath = hotelDTO.ImagePath,
                ApplicationUserId = result.UserId,
                Location = hotelDTO.Location,
                Governorate = hotelDTO.Governorate
            };

            await _unitOfWork.Hotels.CreateHotelAsync(myHotelDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            //HotelOutputDTO hoteldTo = _unitOfWork.Hotels.ToHotelOutputDto(hotel);
            return StatusCode(201, result);
        }
        [HttpPut("{hotelId:int}")]
        public IActionResult UpdateHotel([FromRoute] int hotelId, [FromForm] HotelDTO hotelDTO)
        {
            if (hotelDTO == null)
                return BadRequest(ModelState);

            if (!_unitOfWork.Hotels.Exist(hotelId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           // var hotel = _unitOfWork.Hotels.GetById(hotelId);

            HotelOutputDTO h = _unitOfWork.Hotels.UpdateHotel(hotelId,hotelDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

           // hotelDTO.Id = hotel!.Id;

            return Ok(h);
        }
        [HttpDelete("{hotelId:int}")]
        public IActionResult DeleteHotel([FromRoute] int hotelId)
        {
            if (!_unitOfWork.Hotels.Exist(hotelId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.Hotels.DeleteHotel(hotelId!);

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
