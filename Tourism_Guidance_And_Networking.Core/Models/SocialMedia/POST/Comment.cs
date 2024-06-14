
namespace Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
        public double Rate { get; set; }
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        [JsonIgnore]
        public ApplicationUser User { get; set; } = default!;

        public string CreationDate { get; set; } = string.Empty;
        [Required]
        public int PostId { get; set; } 
        [ForeignKey("PostId")]
        [ValidateNever]
        [JsonIgnore]
        public Post Post { get; set; } = default!;
        [JsonIgnore]
        [ValidateNever]
        public ICollection<CommentLikes> Likes { get; set; } = default!;

    }
}
