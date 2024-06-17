using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST;

namespace Tourism_Guidance_And_Networking.Web.Controllers.SocialMediaControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAllCommentsByPostId/{postId}")]
        public async Task<IActionResult> GetAllCommentsByPostId(int postId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var post = await _unitOfWork.Posts.GetByIdAsync(postId);

            if (post == null)
                return NotFound("Post Not Found");

            var comments = await _unitOfWork.Comments.GetAllCommentsByPostId(postId);

            return Ok(comments);
        }
        [HttpGet("GetCommentLikeStatus/{commentId}")]
        public async Task<IActionResult> GetCommentLikeStatus(int commentId, [FromQuery] string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Id == userId);
            var comment = await _unitOfWork.Comments.FindAsync(p => p.Id == commentId && p.ApplicationUserId == userId);

            if (user is null || comment is null)
            {
                return NotFound();
            }
            var status = await _unitOfWork.Comments.GetCommentLikeStatus(commentId, userId);

            return Ok(status);

        }
        [HttpGet("GetAllCommentsByUserId")]
        public async Task<IActionResult> GetAllCommentsByUserId([FromQuery] string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.ApplicationUsers.FindAsync(a => a.Id == userId);

            if (user == null)
                return NotFound("User Not Found");

            var comments = await _unitOfWork.Comments.GetAllCommentsByUserId(userId);

            return Ok(comments);
        }
        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment(CommentInputDTO commentInput)
        {
            if (commentInput == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.ApplicationUsers.FindAsync(a => a.Id == commentInput.UserId);
            var post = await _unitOfWork.Posts.GetByIdAsync(commentInput.PostID);

            if(user == null || post is null)
                return BadRequest(ModelState);

            Comment comment = await _unitOfWork.Comments.CreateCommentAsync(commentInput);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok(comment);
        }
        [HttpPost("CreateCommentLike")]
        public async Task<IActionResult> CreateCommentLike(CommentLikeDTO commentLikeDTO)
        {
            if (commentLikeDTO == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.ApplicationUsers.FindAsync(a => a.Id == commentLikeDTO.ApplicationUserId);
            var comment = await _unitOfWork.Comments.FindAsync(c => c.Id == commentLikeDTO.CommentId);

            if(user == null || comment is null)
                return BadRequest(ModelState);

            CommentLikes commentLikes = await _unitOfWork.Comments.CreateCommentLikeAsync(commentLikeDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok(commentLikes);
        }
        [HttpPut("UpdateComment/{id}")]
        public async Task<IActionResult> UpdateComment(int id, CommentInputDTO commentInputDTO)
        {
            var comment = await _unitOfWork.Comments.FindAsync(c => c.Id ==  id);

            if (comment == null)
                return NotFound("Comment Not Comment");

            if (commentInputDTO == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.ApplicationUsers.FindAsync(a => a.Id == commentInputDTO.UserId);
            var post = await _unitOfWork.Posts.GetByIdAsync(commentInputDTO.PostID);

            if (user == null || post is null)
                return BadRequest(ModelState);

            comment = await _unitOfWork.Comments.UpdateCommentAsync(id, commentInputDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok(comment);
        }
        [HttpPut("UpdateCommentLike")]
        public async Task<IActionResult> UpdateCommentLike(CommentLikeDTO commentLikeDTO)
        {
            if (commentLikeDTO == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.ApplicationUsers.FindAsync(a => a.Id == commentLikeDTO.ApplicationUserId);
            var comment = await _unitOfWork.Comments.FindAsync(c => c.Id == commentLikeDTO.CommentId);

            if (user == null || comment is null)
                return BadRequest(ModelState);

            CommentLikes commentLike = await _unitOfWork.Comments.UpdateCommentLikeAsync(commentLikeDTO);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok(commentLike);
        }
        [HttpDelete("DeleteComment/{id}")]
        public IActionResult DeleteComment(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = _unitOfWork.Comments.GetById(id);

            if (comment is null)
                return BadRequest(ModelState);

            _unitOfWork.Comments.DeleteComment(id);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
        [HttpDelete("DeleteCommentLike/{id}")]
        public IActionResult DeleteCommentLike(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool ok = _unitOfWork.Comments.DeleteCommentLike(id);

            if(!ok)
                return NotFound(ModelState);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }

}
