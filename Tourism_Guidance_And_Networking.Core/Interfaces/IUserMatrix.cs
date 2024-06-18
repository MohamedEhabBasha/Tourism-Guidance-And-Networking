namespace Tourism_Guidance_And_Networking.Core.Interfaces;

public interface IUserMatrix : IBaseRepository<UserMatrix>
{
    List<UserMatrix> CreateAllUserMatrices();
}
