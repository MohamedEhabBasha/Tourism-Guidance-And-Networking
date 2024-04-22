namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class CommentInputDTO
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public int PostID { get; set; }
        public double Rate { get; set; }
    }
}
