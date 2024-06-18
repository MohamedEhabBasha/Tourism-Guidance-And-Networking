using System.ComponentModel.DataAnnotations;

namespace Tourism_Guidance_And_Networking.Core.Models
{
    public class UserMatrix
    {
        [Required]
        public string UserID {  get; set; }
        [Required]
        public int ItemID { get; set; }
        [Required]
        public string Action { get; set; }
    }
}
