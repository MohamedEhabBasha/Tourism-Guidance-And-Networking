using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism_Guidance_And_Networking.Core.DTOs.AI_Integration
{
    public class InteractionResult
    {

        public List<RoomsInteractions> RoomsInteractions { get; set; }
        public List<AccomdationsIneractions> AccomdationsIneractions { get; set; }
    }
}
