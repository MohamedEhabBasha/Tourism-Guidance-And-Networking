﻿

namespace Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces
{
    public interface ITouristPlaceRepository : IBaseRepository<TouristPlace>
    {
        Task<TouristPlace> CreateTouristPlace(TouristPlaceDTO touristPlaceDTO);
        Task<ICollection<TouristPlace>> SearchByName(string name);
        Task<TouristPlace?> UpdateTouristPlace(int touristId, TouristPlaceDTO touristPlaceDTO);
        bool DeleteTouristPlace(int id);
    }
}
