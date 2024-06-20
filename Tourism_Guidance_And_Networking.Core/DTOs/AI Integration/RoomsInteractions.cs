using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.DTOs.AI_Integration
{
    public  class RoomsInteractions
    {
        public string Action { get; set; }
        public Room Room { get; set; }
        public Hotel Hotel { get; set; }
    }
}
