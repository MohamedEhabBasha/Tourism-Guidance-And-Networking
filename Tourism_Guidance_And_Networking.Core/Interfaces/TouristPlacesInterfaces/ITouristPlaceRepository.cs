using Tourism_Guidance_And_Networking.Core.DTOs;
using Tourism_Guidance_And_Networking.Core.Models.TouristPlaces;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces
{
    public interface ITouristPlaceRepository : IBaseRepository<TouristPlace>
    {
        Task<TouristPlace> CreateTouristPlace(TouristPlaceDTO touristPlaceDTO);
        Task<ICollection<TouristPlace>> SearchByName(String name);
        Task<TouristPlace?> UpdateTouristPlace(TouristPlaceDTO touristPlaceDTO);

        bool DeleteTouristPlace(int id);
    }
}
