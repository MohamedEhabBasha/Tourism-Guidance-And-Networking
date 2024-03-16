
namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class UserProfileDTO
    {
        public ApplicationUser User { get; set; } = default!;
        public List<ApplicationUser> Friends { get; set; } = default!;
        //public bool IsFriend { get; set; } = false;
    }
}
