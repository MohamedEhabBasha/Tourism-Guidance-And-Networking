

namespace Tourism_Guidance_And_Networking.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();
    }
}
