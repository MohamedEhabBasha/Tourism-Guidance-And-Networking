namespace Tourism_Guidance_And_Networking.Web.Services.AI
{
    public interface IExternalService
    {
        Task<string> PostDataToBackendAsync(List<CommentsDTO> objects);
    }
}
