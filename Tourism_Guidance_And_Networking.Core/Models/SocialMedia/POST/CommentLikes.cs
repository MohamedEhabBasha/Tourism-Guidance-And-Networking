
namespace Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST
{
    public class CommentLikes
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
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        [ValidateNever]
        [JsonIgnore]
        public Comment Comment { get; set; } = default!;
    }
}
