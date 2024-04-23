using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;
using Tourism_Guidance_And_Networking.Core.Interfaces.SocialMedia;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.SocialMediaRepositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly string _imagesPath;
        public PostRepository(ApplicationDbContext context, IImageService environment) : base(context)
        {
            _context = context;
            _imageService = environment;
            _imagesPath = FileSettings.postImagesPath;
        }
        public async Task<ICollection<PostDTO>> GetAllPosts()
        {
            List<PostDTO> postDTOs = new();

            var posts = await GetAllAsync();

            foreach (var post in posts)
            {
                PostDTO postDTO = await PostToPostDTO(post);

                postDTOs.Add(postDTO);
            }

            return postDTOs;
        }

        public async Task<ICollection<PostDTO>> GetAllPostsByUserId(string id)
        {
            List<PostDTO> postDTOs = new();

            var posts = await FindAllAsync(x => x.ApplicationUserId == id);

            foreach (var post in posts)
            {
                PostDTO postDTO = await PostToPostDTO(post);

                postDTOs.Add(postDTO);
            }

            return postDTOs;
        }
        public async Task<ICollection<PostDTO>> GetAllFriendsPostsByUserId(string id)
        {
            List<PostDTO> postDTOs = new();

            Func<Friend, string> func = f => f.AppFriendId == id? f.AppUserId:f.AppFriendId;

            var friends = await _context.Friends.Where(f => f.AppFriendId == id || f.AppUserId == id).Select(f => func(f)).ToListAsync();

            var posts = await _context.Posts.Where(p => friends.Contains(p.ApplicationUserId)).ToListAsync();

            foreach (var post in posts)
            {
                PostDTO postDTO = await PostToPostDTO(post);

                postDTOs.Add(postDTO);
            }

            return postDTOs;
        }
        public async Task<Post> CreatePostAsync(PostInputDTO postDTO)
        {
            Post post = new()
            {
                Description = postDTO.Description,
                ApplicationUserId = postDTO.UserId
            };

            var fileResult = _imageService.SaveImage(postDTO.ImagePath, _imagesPath);

            if (fileResult.Item1 == 1)
            {
                post.Image = fileResult.Item2;
            }

            return await AddAsync(post);
        }
        public async Task<Post?> UpdatePostAsync(int postId, PostInputDTO postDTO)
        {
            var post = await FindAsync(x => x.Id ==  postId);

            if (post is null)
                return null;

            #region OldWayImageSave
            //bool hasNewCover = postDTO.ImagePath is not null;
            //bool equal = Equal(post, postDTO);
            //var oldCover = post.Image;

            //post.Description = postDTO.Description;
            //post.ApplicationUserId = postDTO.UserId;

            //if (hasNewCover)
            //{
            //    post.Image = await SaveCover(postDTO.ImagePath!, _imagesPath);
            //    equal = oldCover == post.Image;
            //}
            //if (!equal)
            //{
            //    if (hasNewCover)
            //    {
            //        var cover = Path.Combine(_imagesPath, oldCover);
            //        File.Delete(cover);
            //    }

            //    return post;
            //}
            //else
            //{
            //    //var cover = Path.Combine(_imagesPath, touristPlace.Image);
            //    //File.Delete(cover);

            //    return post;
            //} 
            #endregion

            string oldImage = post.Image;

            if(postDTO.ImagePath is not null)
            {
                var fileResult = _imageService.SaveImage(postDTO.ImagePath, _imagesPath);

                if (fileResult.Item1 == 1)
                {
                    post.Image = fileResult.Item2;
                }
            }

            post.Description = postDTO.Description;
            post.ApplicationUserId = postDTO.UserId;

            if (postDTO.ImagePath is not null)
            {
                _imageService.DeleteImage(oldImage, _imagesPath);
            }

            return Update(post);
        }
        public bool DeletePost(int postId)
        {
            var post = GetById(postId);

            if (post == null)
                return false;

            var postLikes = _context.PostLikes.Where(p => p.PostId ==  postId).ToList();
            var comments = _context.Comments.Where(c => c.PostId  == postId).ToList();

            _context.PostLikes.RemoveRange(postLikes);
            _context.Comments.RemoveRange(comments);

            Delete(post);
            _imageService.DeleteImage(post.Image, _imagesPath);

            return true;
        }
        public async Task<PostLikes> CreatePostLike(PostLikeDTO postLikeDTO)
        {
            PostLikes postLikes = new()
            {
                PostId = postLikeDTO.PostId,
                ApplicationUserId = postLikeDTO.ApplicationUserId,
                IsLiked = postLikeDTO.IsLiked
            };
            await _context.PostLikes.AddAsync(postLikes);

            return postLikes;
        }
        public PostLikes UpdatePostLike(PostLikeDTO postLikeDTO)
        {
            var postLike = _context.PostLikes.First(p => p.PostId == postLikeDTO.PostId && p.ApplicationUserId == postLikeDTO.ApplicationUserId);

            postLike.IsLiked = postLikeDTO.IsLiked;

            _context.PostLikes.Update(postLike);

            return postLike;
        }
        public bool DeletePostLikes(int postId, string userId)
        {
            var postLike = _context.PostLikes.First(p => p.PostId == postId && p.ApplicationUserId == userId);

            _context.PostLikes.Remove(postLike);

            return true;
        }
        private async Task<PostDTO> PostToPostDTO(Post post)
        {
            PostDTO postDTO = new() { PostId = post.Id, Description = post.Description, Image = $"{FileSettings.RootPath}/{_imagesPath}/{post.Image}"};

            var comments = await _context.Comments.Where(c => c.PostId == post.Id).ToListAsync();
            List<CommentDTO> commentsDTO = new ();

            foreach (var comment in comments)
            {
                CommentDTO commentDTO = await CommentToCommentDTO(comment);
                commentsDTO.Add(commentDTO);
            }

            var user = await _context.ApplicationUsers.SingleAsync(u => u.Id == post.ApplicationUserId);

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
            
            postDTO.UserDTO = userDTO;
            postDTO.Comments = commentsDTO;
            postDTO.TotalComments = commentsDTO.Count;

            var totalLikes = _context.PostLikes.Where(p => p.PostId == post.Id).Count(c => c.IsLiked);
            var totalDisLikes = _context.PostLikes.Where(p => p.PostId == post.Id).Count(c => !c.IsLiked);

            postDTO.TotalLikes = totalLikes;
            postDTO.TotalDisLikes = totalDisLikes;

            return postDTO;
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

            return commentDTO;
        }

        public async Task<int> GetPostLikeStatus(int postId, string userId)
        {
            var postLike = await _context.PostLikes.SingleOrDefaultAsync(c => c.PostId ==  postId && c.ApplicationUserId == userId);

            if (postLike == null)
                return 0;
            else if (postLike.IsLiked) { return 1; }

            return -1;

        }
    }
}
