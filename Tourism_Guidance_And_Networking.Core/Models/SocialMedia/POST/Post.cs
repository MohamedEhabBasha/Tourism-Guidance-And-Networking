namespace Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Image { get; set; } = string.Empty;
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        [JsonIgnore]
        public ApplicationUser User { get; set; } = default!;
        [JsonIgnore]
        [ValidateNever]
        public ICollection<Comment> Comments { get; set; } = default!;
        [JsonIgnore]
        [ValidateNever]
        public ICollection<PostLikes> Likes { get; set; } = default!;
    }
}
