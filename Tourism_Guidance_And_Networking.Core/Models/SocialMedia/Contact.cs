
namespace Tourism_Guidance_And_Networking.Core.Models.SocialMedia
{
    public class Contact
    {
        public int Id { get; set; }
        [Required]
        public string AppUserId { get; set; } = string.Empty;
        [Required]
        public string AppFriendId { get; set; } = string.Empty;
    }
}
