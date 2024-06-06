
using Microsoft.AspNetCore.Cors;
using System.Runtime.InteropServices;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST;

namespace Tourism_Guidance_And_Networking.Web.Controllers.SocialMediaControllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _unitOfWork.Posts.GetAllPosts();

            return Ok(posts);
        }
        [HttpGet("GetPostById/{id:int}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);

            if(post is null)
                return NotFound();

            return Ok(await _unitOfWork.Posts.GetPostById(id));
        }
        [HttpGet("GetAllPostsByUserId")]
        public async Task<IActionResult> GetAllPostsByUserId([FromQuery] string userId)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == userId);

            if (user is null)
            {
                return NotFound();
            }

            var posts = await _unitOfWork.Posts.GetAllPostsByUserId(userId);

            return Ok(posts);
        }
        [HttpGet("GetAllFriendsPostsByUserId")]
        public async Task<IActionResult> GetAllFriendsPostsByUserId([FromQuery] string userId)
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == userId);

            if (user is null)
            {
                return NotFound();
            }

            var posts = await _unitOfWork.Posts.GetAllFriendsPostsByUserId(userId);

            return Ok(posts);
        }
        [HttpGet("GetPostLikeStatus/{postId}")]
        public async Task<IActionResult> GetPostLikeStatus(int postId,[FromQuery] string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == userId);
            var post = await _unitOfWork.Posts.FindAsync(p => p.Id == postId && p.ApplicationUserId == userId);

            if (user is null || post is null)
            {
                return NotFound();
            }
            int status = await _unitOfWork.Posts.GetPostLikeStatus(postId, userId);

            return Ok(status);

        }
        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost([FromForm] PostInputDTO postDTO) 
        {
            if (postDTO == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == postDTO.UserId);

            if (user is null)
            {
                return NotFound();
            }

            Post post = await _unitOfWork.Posts.CreatePostAsync(postDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, post);
        }
        [HttpPut("UpdatePost/{id}")]
        public async Task<IActionResult> UpdatePost(int id,[FromForm] PostInputDTO postDTO)
        {
            if (postDTO == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == postDTO.UserId);

            if (user is null)
            {
                return NotFound();
            }

            var post = await _unitOfWork.Posts.UpdatePostAsync(id, postDTO);

            if (post == null)
                return BadRequest("No existing post");

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return StatusCode(201, post);
        }
        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool ok =  _unitOfWork.Posts.DeletePost(id);

            if(!ok)
                return BadRequest(ModelState);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
        [HttpPost("CreatePostLike")]
        public async Task<IActionResult> CreatePostLike(PostLikeDTO postLikeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = await _unitOfWork.Posts.FindAsync(p => p.Id == postLikeDTO.PostId);
            var user = await _unitOfWork.ApplicationUsers.FindAsync(a => a.Id == postLikeDTO.ApplicationUserId);

            if(post is null || user is null)
                return BadRequest(ModelState);

            var postLike = await _unitOfWork.Posts.CreatePostLike(postLikeDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok(postLike);
        }
        [HttpPut("UpdatePostLike")]
        public async Task<IActionResult> UpdatePostLike(PostLikeDTO postLikeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post =  _unitOfWork.Posts.GetById(postLikeDTO.PostId);
            var user = await _unitOfWork.ApplicationUsers.FindAsync(a => a.Id == postLikeDTO.ApplicationUserId); // need to be checked

            if (post is null || user is null)
                return BadRequest(ModelState);

            var postLike = _unitOfWork.Posts.UpdatePostLike(postLikeDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok(postLike);
        }
        [HttpDelete("DeletePostLike/{postId}")]
        public async Task<IActionResult> DeletePostLike(int postId, string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = _unitOfWork.Posts.GetById(postId);
            var user = await _unitOfWork.ApplicationUsers.FindAsync(a => a.Id == userId);

            if (post is null || user is null)
                return BadRequest(ModelState);

            _unitOfWork.Posts.DeletePostLikes(postId, userId);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}
