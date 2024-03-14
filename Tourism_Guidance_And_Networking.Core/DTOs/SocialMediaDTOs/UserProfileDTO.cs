
namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class UserProfileDTO
    {
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public List<ApplicationUser> Friends { get; set; } = default!;
        public bool AreFriends { get; set; } = false;
    }
}
