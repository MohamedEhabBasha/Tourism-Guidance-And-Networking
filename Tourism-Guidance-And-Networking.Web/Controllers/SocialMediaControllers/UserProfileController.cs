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
        public async Task<IActionResult> GetAllContacts([FromQuery] string userName)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.UserName == userName);

            if (user == null)
            {
                return NotFound();
            }
            var contacts = await _unitOfWork.UserProfiles.GetAllContacts(userName);

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

            var userProfile = await _unitOfWork.UserProfiles.GetUserProfileDTOAsync(user.UserName!);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(userProfile);
        }
        [HttpGet("IsFriend")]
        public async Task<IActionResult> IsFriend([FromQuery]string userName,[FromQuery]string friendName)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.UserName == userName);
            var friend = await _unitOfWork.ApplicationUsers.FindAsync(x => x.UserName == friendName);

            if (user == null || friend is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool isFriend = await _unitOfWork.UserProfiles.IsFriendAsync(userName, friendName);

            return Ok(isFriend);
        }
        [HttpPost("createFriends")]
        public async Task<IActionResult> CreateFriend([FromBody] FriendDTO friendDTO)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.UserName == friendDTO.UserName);
            var friend = await _unitOfWork.ApplicationUsers.FindAsync(x => x.UserName == friendDTO.FriendName);

            if (user == null || friend is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exist = await _unitOfWork.UserProfiles.GetFriendAsync(user.UserName!, friend.UserName!);

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
        public IActionResult DeleteFriend([FromQuery] string userName, [FromQuery] string friendName)
        {
            var user = _unitOfWork.ApplicationUsers.FindAsync(x => x.UserName == userName);

            var friend = _unitOfWork.ApplicationUsers.FindAsync(x => x.UserName == friendName);
            if (user == null || friend is null)
            {
                return NotFound();
            }
            if (userName == friendName)
            {
                return BadRequest();
            }
            _unitOfWork.UserProfiles.DeleteFriend(userName, friendName);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }
        [HttpDelete("DeleteContact")]
        public IActionResult DeleteContacts([FromQuery] string userName, [FromQuery] string friendName)
        {
            var user = _unitOfWork.ApplicationUsers.FindAsync(x => x.UserName == userName);
            var friend = _unitOfWork.ApplicationUsers.FindAsync(x => x.UserName == friendName);
            if (user == null || friend is null)
            {
                return NotFound();
            }
            if (userName == friendName)
            {
                return BadRequest();
            }
            _unitOfWork.UserProfiles.DeleteContact(userName, friendName);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }
    }
}
