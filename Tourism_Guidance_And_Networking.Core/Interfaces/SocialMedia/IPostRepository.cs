using Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.SocialMedia
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<ICollection<PostDTO>> GetAllPosts();
        Task<PostDTO> GetPostByIdAsync(int id);
        Task<ICollection<PostDTO>> GetAllPostsByUserId(string id);
        Task<ICollection<PostDTO>> GetAllFriendsPostsByUserId(string id);
        Task<int> GetPostLikeStatus(int postId, string userId);
        Task<Post> CreatePostAsync(PostInputDTO postDTO);
        Post UpdatePost(int postId,PostInputDTO postDTO);
        bool DeletePost(int postId);
        Task<PostLikes> CreatePostLike(PostLikeDTO postLikeDTO);
        PostLikes UpdatePostLike(PostLikeDTO postLikeDTO);
        bool DeletePostLikes(int postId, string userId);

    }
}
