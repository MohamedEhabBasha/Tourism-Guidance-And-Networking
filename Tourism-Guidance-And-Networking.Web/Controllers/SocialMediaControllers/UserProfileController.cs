using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;

namespace Tourism_Guidance_And_Networking.Web.Controllers.SocialMediaControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAllFriends")]
        public async Task<IActionResult> GetAllFriends([FromQuery] string id)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            var friends = await _unitOfWork.UserProfiles.GetAllFriends(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(friends);
        }
        [HttpGet("GetAllContacts")]
        public async Task<IActionResult> GetAllContacts([FromQuery] string id)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            var contacts = await _unitOfWork.UserProfiles.GetAllContacts(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(contacts);
        }
        [HttpPost("createFriends")]
        public async Task<IActionResult> CreateFriend(string userId, string friendId)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == userId);
            var friend = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == friendId);

            if (user == null || friend is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exist = await _unitOfWork.UserProfiles.GetFriendAsync(userId, friendId);

            if (exist is not null || (userId == friendId))
            {
                return BadRequest("Already created or two ids are equals");
            }

            Friend newFriend = new() {
                AppUserId = userId,
                AppFriendId = friendId
            };

            await _unitOfWork.UserProfiles.CreateFriendAsync(newFriend);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, newFriend);
        }

        [HttpDelete("DeleteFriend")]
        public IActionResult DeleteFriend(string userId, string friendId)
        {
            var user = _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == userId);
            var friend = _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == friendId);
            if (user == null || friend is null)
            {
                return NotFound();
            }
            if (userId == friendId)
            {
                return BadRequest();
            }
            _unitOfWork.UserProfiles.DeleteFriend(userId, friendId);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }
        [HttpDelete("DeleteContact")]
        public IActionResult DeleteContacts(string userId, string friendId)
        {
            var user = _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == userId);
            var friend = _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == friendId);
            if (user == null || friend is null)
            {
                return NotFound();
            }
            if (userId == friendId)
            {
                return BadRequest();
            }
            _unitOfWork.UserProfiles.DeleteContact(userId, friendId);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }
    }
}
