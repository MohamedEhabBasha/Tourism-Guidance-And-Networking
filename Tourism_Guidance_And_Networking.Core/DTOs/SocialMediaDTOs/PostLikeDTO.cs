namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class PostLikeDTO
    {
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        [Required]
        public int PostId { get; set; }
        [Required]
        public bool IsLiked { get; set; }
    }
}
