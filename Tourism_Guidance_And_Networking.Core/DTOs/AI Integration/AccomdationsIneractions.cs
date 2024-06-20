using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.DTOs.AI_Integration
{
    public  class AccomdationsIneractions
    {
        public string Action { get; set; }
        public Accommodation Accommodation { get; set; }
    }
}
