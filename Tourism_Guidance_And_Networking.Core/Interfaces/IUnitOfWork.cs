

using Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces;

namespace Tourism_Guidance_And_Networking.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        ITouristPlaceRepository Tours { get; }
        int Complete();
    }
}
