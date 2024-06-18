using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism_Guidance_And_Networking.Core.Models
{
    public  class Data
    {
        public Dictionary<string, string> Name { get; set; }
        public Dictionary<string, string> type { get; set; }
        public Dictionary<string, string> address { get; set; }
        public Dictionary<string, string> price { get; set; }
        public Dictionary<string, string> taxes_and_charges { get; set; }
        public Dictionary<string, string> rating { get; set; }
        public Dictionary<string, string> number_of_reviews { get; set; }
        public Dictionary<string, string> info { get; set; }
        public Dictionary<string, string> img { get; set; }
        public Dictionary<string, string> property_type { get; set; }
        public Dictionary<string, string> num_adults { get; set; }
        public Dictionary<string, string> location { get; set; }
        public Dictionary<string, string> description { get; set; }
        public Dictionary<string, string> governorate { get; set; }
    }
}
