
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.DTOs;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Web.Controllers.HotelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("hotels")]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _unitOfWork.Hotels.GetAllAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(hotels);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(hotel);
        }
        [HttpGet("hotelByName")]
        public async Task<IActionResult> GetHotelByName(string name)
        {
            var hotel = await _unitOfWork.Hotels.GetHotelByNameAsync(name);

            if (hotel is null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(hotel);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateHotel([FromForm]HotelDTO hotelDTO)
        {
            if (hotelDTO == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Hotel hotel = await _unitOfWork.Hotels.CreateHotelAsync(hotelDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return StatusCode(201, hotel);
        }
        [HttpPut("{hotelId:int}")]
        public async Task<IActionResult> UpdateHotel([FromRoute] int hotelId, [FromForm] HotelDTO hotelDTO)
        {
            if (hotelDTO == null)
                return BadRequest(ModelState);

            if (!_unitOfWork.Hotels.Exist(hotelId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = _unitOfWork.Hotels.GetById(hotelId);

            await _unitOfWork.Hotels.UpdateHotel(hotelId,hotelDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

           // hotelDTO.Id = hotel!.Id;

            return Ok(hotel);
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
    }
}
