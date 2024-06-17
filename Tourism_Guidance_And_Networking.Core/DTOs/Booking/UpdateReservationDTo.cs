using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.DTOs.Booking
{
    public class UpdateReservationDTo
    {
        public int Count { get; set; }
        public int? AccommodationId { get; set; } = null;
        public int? RoomId { get; set; } = null;
        public double Price { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
