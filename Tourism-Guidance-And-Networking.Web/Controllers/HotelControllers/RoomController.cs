
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Web.Controllers.HotelControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);

            if (room == null)
            {
                return NotFound("Room do not exist!!");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roomDto = await _unitOfWork.Rooms.GetRoomById(id);
            return Ok(roomDto);
        }
        [HttpGet("rooms/{hotelId:int}")]
        public async Task<IActionResult> GetRoomsByHotelId(int hotelId)
        {
            if (!_unitOfWork.Hotels.Exist(hotelId))
                return NotFound();

            var rooms = await _unitOfWork.Rooms.GetRoomsByHotelIdAsync(hotelId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rooms);
        }
        [HttpGet("roomsByRoomType/{id:int}")]
        public async Task<IActionResult> GetRoomsByRoomType(string type, int id)
        {
            if (!_unitOfWork.Hotels.Exist(id))
                return NotFound();
            if (!_unitOfWork.Rooms.TypeExist(type))
                return NotFound();

            var rooms = await _unitOfWork.Rooms.GetRoomsByTypeAsync(type, id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rooms);
        }
        [HttpGet("roomsSearchByName")]
        public async Task<IActionResult> GetRoomsBySearchName(string name)
        {
            var rooms = await _unitOfWork.Rooms.SearchRoomByNameAsync(name.Trim().ToLower());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rooms);
        }
        [HttpGet("FilterRoomsByPrice")]
        public async Task<IActionResult> FilterRoomByPrice([FromQuery] double price)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _unitOfWork.Rooms.FilterByPrice(price));
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateRoom([FromForm] RoomDTO roomDTO)
        {
            if (roomDTO == null || !_unitOfWork.Hotels.Exist(roomDTO.HotelId))
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = await _unitOfWork.Rooms.CreateRoomAsync(roomDTO);


            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            RoomOutputDTO output = new()
            {
                ID = room.Id,
                Type = room.Type,
                Price = room.Price,
                Taxes = room.Taxes,
                Info = room.Info,
                Description = room.Description,
                Capicity = room.Capicity,
                HotelId = room.HotelId,
                Count = room.Count,
                ImageURL = $"{FileSettings.RootPath}/{FileSettings.roomImagesPath}/{room.Image}"
            };
            return StatusCode(201, output);
        }
        
        [HttpPost("CreaterRoomFromHotel")]
        [Authorize(Roles=Roles.Hotel)]
        public async Task<IActionResult> CreateRoomFromHotel([FromForm] CreateRoomFromHotelDTO roomDTO)
        {
            if (roomDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            var hotel = await _unitOfWork.Hotels.FindAsync(h => h.ApplicationUserId == applicationUser.Id);

            if (hotel is null)
                return BadRequest("The Current User is not an hotel user");

            RoomDTO roomDto = new()
            {
                Type = roomDTO.Type,
                Price = roomDTO.Price,
                Count = roomDTO.Count,
                Taxes = roomDTO.Taxes,
                Info = roomDTO.Info,
                Description = roomDTO.Description,
                ImagePath = roomDTO.ImagePath,
                Capicity = roomDTO.Capicity,
                HotelId = hotel.Id
            };

            var room = await _unitOfWork.Rooms.CreateRoomAsync(roomDto);


            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            RoomOutputDTO output = new()
            {
                ID = room.Id,
                Type = room.Type,
                Price = room.Price,
                Taxes = room.Taxes,
                Info = room.Info,
                Description = room.Description,
                Capicity = room.Capicity,
                HotelId = room.HotelId,
                Count = room.Count,
                ImageURL = $"{FileSettings.RootPath}/{FileSettings.roomImagesPath}/{room.Image}"
            };
            return StatusCode(201, output);
        }
        
        [HttpPut("{roomId:int}")]
        public IActionResult UpdateRoom([FromRoute] int roomId, [FromForm] RoomDTO roomDTO)
        {
            if (roomDTO == null || !_unitOfWork.Hotels.Exist(roomDTO.HotelId))
                return BadRequest(ModelState);

            if (!_unitOfWork.Rooms.Exist(roomId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = _unitOfWork.Rooms.GetById(roomId);

            var output = _unitOfWork.Rooms.UpdateRoom(roomId,roomDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

          //  roomDTO.Id = room!.Id;

            return Ok(output);
        }
        [HttpDelete("{roomId:int}")]
        public IActionResult DeleteRoom([FromRoute] int roomId)
        {
            if (!_unitOfWork.Rooms.Exist(roomId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _unitOfWork.Rooms.DeleteRoom(roomId);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }
    }
}
