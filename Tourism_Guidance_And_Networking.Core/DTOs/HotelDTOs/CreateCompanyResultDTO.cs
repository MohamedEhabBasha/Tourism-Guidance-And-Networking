using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public  class CreateCompanyResultDTO
    {
        public int CompanyId { get; set; }
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        //public DateTime ExpiresOn { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefrshTokenExpiration { get; set; }
    }
}
