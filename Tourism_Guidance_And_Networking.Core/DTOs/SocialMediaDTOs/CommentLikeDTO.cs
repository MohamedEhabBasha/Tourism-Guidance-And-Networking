namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class CommentLikeDTO
    {
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        [Required]
        public int CommentId { get; set; }
        [Required]
        public bool IsLiked { get; set; }
    }
}
