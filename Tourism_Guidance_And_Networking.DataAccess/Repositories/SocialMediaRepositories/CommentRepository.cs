using Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;
using Tourism_Guidance_And_Networking.Core.Interfaces.SocialMedia;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.SocialMediaRepositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private new readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ICollection<CommentDTO>> GetAllCommentsByPostId(int postId)
        {
            List<CommentDTO> commentDTOs = new();

            var comments = await FindAllAsync(c => c.PostId == postId);

            foreach (var comment in comments)
            {
                CommentDTO commentDTO = await CommentToCommentDTO(comment);

                commentDTOs.Add(commentDTO);
            }
            return commentDTOs;
        }

        public async Task<ICollection<CommentDTO>> GetAllCommentsByUserId(string userId)
        {
            List<CommentDTO> commentDTOs = new();

            var comments = await _context.Comments.Where(c => c.ApplicationUserId == userId).ToListAsync();

            foreach (var comment in comments)
            {
                CommentDTO commentDTO = await CommentToCommentDTO(comment);

                commentDTOs.Add(commentDTO);
            }
            return commentDTOs;
        }
        public async Task<Comment> CreateCommentAsync(CommentInputDTO commentDTO)
        {
            Comment comment = new()
            {
                Text = commentDTO.Text,
                ApplicationUserId = commentDTO.UserId,
                PostId = commentDTO.PostID,
                Rate = commentDTO.Rate,
                CreationDate = DateTime.Now.ToString()
            };

            return await AddAsync(comment);
        }

        public async Task<CommentLikes> CreateCommentLikeAsync(CommentLikeDTO commentLikeDTO)
        {
            CommentLikes commentLikes = new()
            {
                ApplicationUserId = commentLikeDTO.ApplicationUserId,
                CommentId = commentLikeDTO.CommentId,
                IsLiked = commentLikeDTO.IsLiked
            };

            await _context.CommentLikes.AddAsync(commentLikes);

            return commentLikes;
        }

        public bool DeleteComment(int id)
        {
            var comment = GetById(id);

            var commentLikes = _context.CommentLikes.Where(c => c.CommentId == id).ToList();

            _context.CommentLikes.RemoveRange(commentLikes);
            Delete(comment!);

            return true;
        }

        public bool DeleteCommentLike(int id)
        {
            var commentLike = _context.CommentLikes.SingleOrDefault(c => c.Id  == id);

            if(commentLike == null)
                return false;

            _context.CommentLikes.Remove(commentLike);

            return true;
        }

        public async Task<Comment> UpdateCommentAsync(int id, CommentInputDTO commentDTO)
        {
            var comment = await GetByIdAsync(id);

            comment!.Text = commentDTO.Text;
            comment.ApplicationUserId = commentDTO.UserId;
            comment.PostId = commentDTO.PostID;
            comment.Rate = commentDTO.Rate;

            return Update(comment);
        }

        public async Task<CommentLikes> UpdateCommentLikeAsync(CommentLikeDTO commentLikeDTO)
        {
            var commentLike = await _context.CommentLikes.SingleAsync(c => c.CommentId == commentLikeDTO.CommentId);

            commentLike.IsLiked = commentLikeDTO.IsLiked;

            _context.CommentLikes.Update(commentLike);

            return commentLike;
        }
        private async Task<CommentDTO> CommentToCommentDTO(Comment comment)
        {
            CommentDTO commentDTO = new() { PostId = comment.PostId, Text = comment.Text, Id = comment.Id };

            var user = await _context.ApplicationUsers.SingleAsync(a => a.Id == comment.ApplicationUserId);
            UserDTO userDTO = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            };

            var totalLikes = _context.CommentLikes.Where(c => c.CommentId == comment.Id).Count(c => c.IsLiked);
            var totalDisLikes = _context.CommentLikes.Where(c => c.CommentId == comment.Id).Count(c => !c.IsLiked);

            commentDTO.TotalLikes = totalLikes;
            commentDTO.TotalDisLikes = totalDisLikes;

            commentDTO.ApplicationUser = userDTO;
            commentDTO.Rate = comment.Rate;
            commentDTO.CreationDate = comment.CreationDate;
            return commentDTO;
        }

        public async Task<int> GetCommentLikeStatus(int commentId, string userId)
        {
            var commentLike = await _context.CommentLikes.SingleOrDefaultAsync(c => c.Id == commentId && c.ApplicationUserId == userId);

            if (commentLike == null)
                return 0;
            else if (commentLike.IsLiked) { return 1; }

            return -1;
        }
    }
}
