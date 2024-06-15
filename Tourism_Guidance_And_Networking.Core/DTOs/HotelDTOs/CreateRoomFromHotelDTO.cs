using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class CreateRoomFromHotelDTO
    {

        [MaxLength(250)]
        public string Type { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        public double Taxes { get; set; }
        [Required]
        public string Info { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Image { get; set; } = string.Empty;
        [Required]
        public int Capicity { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int CountOfReserved { get; set; } = 0;
        
    }
}
