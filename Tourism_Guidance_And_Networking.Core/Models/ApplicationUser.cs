
namespace Tourism_Guidance_And_Networking.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(25)]
        public string FirstName { get; set; }
        [MaxLength(25)]
        public string LastName { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
