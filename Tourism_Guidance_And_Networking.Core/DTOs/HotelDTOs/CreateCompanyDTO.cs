using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Attribute;

namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class CreateCompanyDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [MaxLength(250)]
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        public double Rating { get; set; }

        public int Reviews { get; set; }

        [Required(ErrorMessage = "Choose an image"),
        AllowedExtenstions(FileSettings.AllowedExtensions)]
        public IFormFile ImagePath { get; set; } = default!;

    }
}
