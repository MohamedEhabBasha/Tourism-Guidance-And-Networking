

namespace Tourism_Guidance_And_Networking.Core.DTOs
{
    public class BaseDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
    }
}
