using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;

namespace Tourism_Guidance_And_Networking.Web.Controllers.SocialMediaControllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> GetAllContacts([FromQuery] string userId)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == userId);

            if (user == null)
            {
                return NotFound();
            }
            var contacts = await _unitOfWork.UserProfiles.GetAllContacts(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(contacts);
        }
        [HttpGet("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            if (user == null)
            {
                return NotFound();
            }

            var userProfile = await _unitOfWork.UserProfiles.GetUserProfileDTOAsync(user.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(userProfile);
        }
        [HttpGet("IsFriend")]
        public async Task<IActionResult> IsFriend([FromQuery]string userId,[FromQuery]string friendId)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == userId);
            var friend = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == friendId);

            if (user == null || friend is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool isFriend = await _unitOfWork.UserProfiles.IsFriendAsync(userId, friendId);

            return Ok(isFriend);
        }
        [HttpPost("createFriends")]
        public async Task<IActionResult> CreateFriend([FromBody] FriendDTO friendDTO)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == friendDTO.UserId);
            var friend = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == friendDTO.FriendId);

            if (user == null || friend is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exist = await _unitOfWork.UserProfiles.GetFriendAsync(user.Id, friend.Id);

            if (exist is not null || (user.Id == friend.Id))
            {
                return BadRequest("Already created or two ids are equals");
            }



            Friend newFriend = await _unitOfWork.UserProfiles.CreateFriendAsync(friendDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, newFriend);
        }

        [HttpDelete("DeleteFriend")]
        public IActionResult DeleteFriend([FromQuery] string userId, [FromQuery] string friendId)
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
        public IActionResult DeleteContacts([FromQuery] string userId, [FromQuery] string friendId)
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
