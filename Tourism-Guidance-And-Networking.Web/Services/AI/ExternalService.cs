
using System.Text;
using System.Text.Json;

namespace Tourism_Guidance_And_Networking.Web.Services.AI
{
    public class ExternalService : IExternalService
    {
        private readonly HttpClient _httpClient;

        public ExternalService(HttpClient httpClient)
        {

            _httpClient = httpClient;

        }

        public async Task<string> PostDataToBackendAsync(List<CommentsDTO> objects)
        {
            var json = JsonSerializer.Serialize(objects);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/prediction", content); 
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
      
    }
}
