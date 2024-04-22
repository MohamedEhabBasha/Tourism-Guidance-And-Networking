namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public UserDTO ApplicationUser { get; set; } = default!;
        public int PostId { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDisLikes { get; set; }
        public double Rate { get; set; }
    }
}
