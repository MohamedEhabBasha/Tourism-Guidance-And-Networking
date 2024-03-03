
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;

namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class PrivateChatDTO
    {
        public int ChatId { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public IEnumerable<Message> Messages { get; set; } = Enumerable.Empty<Message>();
    }
}
