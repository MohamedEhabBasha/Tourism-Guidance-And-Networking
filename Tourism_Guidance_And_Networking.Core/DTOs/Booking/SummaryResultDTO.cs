using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism_Guidance_And_Networking.Core.DTOs.Booking
{
    public class SummaryResultDTO
    {
        public string SessionId { get; set; }
        public string SessionUrl { get; set; }
        public int BookingNumber { get; set; }
    }
}
