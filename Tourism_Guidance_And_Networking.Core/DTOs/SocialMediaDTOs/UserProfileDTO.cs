
namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class UserProfileDTO
    {
        public UserDTO User { get; set; } = default!;
        public List<UserDTO> Friends { get; set; } = default!;
        //public bool IsFriend { get; set; } = false;
    }
}
