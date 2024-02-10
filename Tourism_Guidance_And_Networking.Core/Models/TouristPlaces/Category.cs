using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace Tourism_Guidance_And_Networking.Core.Models.TouristPlaces
{
    public class Category : BaseEntity
    {
        [ValidateNever]
        [JsonIgnore]
        public ICollection<TouristPlace>? TouristPlaces { get; set; }
    }
}
