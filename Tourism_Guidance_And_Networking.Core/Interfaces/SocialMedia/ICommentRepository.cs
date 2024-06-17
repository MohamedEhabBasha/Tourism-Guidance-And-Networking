using Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST;
namespace Tourism_Guidance_And_Networking.Core.Interfaces.SocialMedia
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<ICollection<CommentDTO>> GetAllCommentsByPostId(int postId);
        Task<ICollection<CommentDTO>> GetAllCommentsByUserId(string userId);
        Task<Comment> CreateCommentAsync(CommentInputDTO commentDTO);
        Task<Comment> UpdateCommentAsync(int id, CommentInputDTO commentDTO);
        Task<StatusDTO> GetCommentLikeStatus(int commentId, string userId);
        bool DeleteComment(int id);
        Task<CommentLikes> CreateCommentLikeAsync(CommentLikeDTO commentLikeDTO);
        Task<CommentLikes> UpdateCommentLikeAsync(CommentLikeDTO commentLikeDTO);
        bool DeleteCommentLike(int commentId, string userId);
    }
}
