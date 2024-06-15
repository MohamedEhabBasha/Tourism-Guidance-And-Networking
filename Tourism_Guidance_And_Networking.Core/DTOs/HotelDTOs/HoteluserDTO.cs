using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class HoteluserDTO
    {
        public ApplicationUser User { get; set; }
        public Hotel Hotel { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
    }
}
