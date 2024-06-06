namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;

public class UserPhotoDTO
{
    public string UserId { get; set; } = string.Empty;

    [NotMapped]
    public IFormFile ImagePath { get; set; } = default!;
}
