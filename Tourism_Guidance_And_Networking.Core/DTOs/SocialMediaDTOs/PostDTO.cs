using Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST;

namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Image {  get; set; } = string.Empty;
        public int TotalLikes { get; set; }
        public int TotlaDisLikes { get; set; }

        public int TotalComments { get; set; }
        public UserDTO UserDTO { get; set; } = default!;
        public IEnumerable<Comment> Comments { get; set; } = Enumerable.Empty<Comment>();
    }
}
