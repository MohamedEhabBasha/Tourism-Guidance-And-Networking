
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Web.Controllers.HotelControllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var hotel = await _unitOfWork.Rooms.GetByIdAsync(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(hotel);
        }
        [HttpGet("rooms/{hotelId:int}")]
        public async Task<IActionResult> GetRoomsByHotelId(int hotelId)
        {
            if (!_unitOfWork.Hotels.Exist(hotelId))
                return NotFound();

            var rooms = await _unitOfWork.Hotels.GetRoomsByIdAsync(hotelId);

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

            var rooms = await _unitOfWork.Hotels.GetRoomsByTypeAsync(type, id);

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
        public async Task<IActionResult> CreateRoom([FromForm] RoomDTO roomDTO)
        {
            if (roomDTO == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Room room = await _unitOfWork.Rooms.CreateRoomAsync(roomDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return StatusCode(201, room);
        }
        [HttpPut("{roomId:int}")]
        public async Task<IActionResult> UpdateRoom([FromRoute] int roomId, [FromBody] RoomDTO roomDTO)
        {
            if (roomDTO == null)
                return BadRequest(ModelState);

            if (!_unitOfWork.Rooms.Exist(roomId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = _unitOfWork.Rooms.GetById(roomId);

            await _unitOfWork.Rooms.UpdateRoom(roomDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            roomDTO.Id = room!.Id;

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
