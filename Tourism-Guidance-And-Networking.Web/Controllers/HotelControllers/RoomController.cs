
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

            if(room == null)
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
        public async Task<IActionResult> GetRoomsByRoomType(string type,int id)
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
        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.Hotel)]
        public async Task<IActionResult> CreateRoom([FromForm] RoomDTO roomDTO)
        {
            if (roomDTO == null || !_unitOfWork.Hotels.Exist(roomDTO.HotelId))
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            RoomOutputDTO room = await _unitOfWork.Rooms.CreateRoomAsync(roomDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return StatusCode(201, room);
        }
        [HttpPut("{roomId:int}")]
        public async Task<IActionResult> UpdateRoom([FromRoute] int roomId, [FromForm] RoomDTO roomDTO)
        {
            if (roomDTO == null || !_unitOfWork.Hotels.Exist(roomDTO.HotelId))
                return BadRequest(ModelState);

            if (!_unitOfWork.Rooms.Exist(roomId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = _unitOfWork.Rooms.GetById(roomId);

            await _unitOfWork.Rooms.UpdateRoom(roomId,roomDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

          //  roomDTO.Id = room!.Id;

            return Ok(room);
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
