using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace Tourism_Guidance_And_Networking.Core.Models.SocialMedia
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
        public string CreatedDate { get; set; } = string.Empty;
        public string ApplicationUserId { get; set; } = string.Empty;
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; } = default!;
        [Required]
        public int ChatId { get; set; }
        [ForeignKey("ChatId")]
        [JsonIgnore]
        [ValidateNever]
        public PrivateChat Chat { get; set; } = default!;
    }
}
