using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism_Guidance_And_Networking.Core.Models.AI_Integration
{
    public class AccomdationMapping
    {
        public int Id { get; set; }
        [Required]
        public int Accomdation { get; set; }
        [Required]
        public int Item { get; set; }
    }
}
