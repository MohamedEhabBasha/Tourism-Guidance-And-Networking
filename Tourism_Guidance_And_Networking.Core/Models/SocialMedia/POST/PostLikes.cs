﻿
namespace Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST
{
    public class PostLikes
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        [JsonIgnore]
        public ApplicationUser User { get; set; } = default!;
        [Required]
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        [ValidateNever]
        [JsonIgnore]
        public Post Post { get; set; } = default!;
        [Required]
        public bool IsLiked { get; set; }
    }
}
