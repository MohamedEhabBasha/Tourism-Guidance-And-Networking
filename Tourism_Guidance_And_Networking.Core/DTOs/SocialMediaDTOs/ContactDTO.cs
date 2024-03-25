
namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class ContactDTO
    {
        public int ChatId {  get; set; }
        public UserDTO User { get; set; } = default!;
    }
}
