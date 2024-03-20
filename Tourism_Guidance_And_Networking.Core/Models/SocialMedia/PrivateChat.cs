using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace Tourism_Guidance_And_Networking.Core.Models.SocialMedia
{
    public class PrivateChat
    {
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; } = string.Empty;
        [Required]
        public string ReceiverId { get; set; } = string.Empty;

        [JsonIgnore]
        [ValidateNever]
        public ICollection<Message> Messages { get; set; } = default!;
    }
}
