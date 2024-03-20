
namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class MessageDTO
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public string SenderEmail { get; set; } = string.Empty;
        [Required]
        public string ReceiverEmail { get; set; } = string.Empty;
        [Required]
        public int ChatId { get; set; }
    }
}
