
namespace Tourism_Guidance_And_Networking.Core.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber {  get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Image {  get; set; } = string.Empty;
        public List<string> Role {  get; set; } = default!;
    }
}
