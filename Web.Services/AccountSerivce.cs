using Web.Services.Interfaces;

namespace Web.Services
{
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Web.Models;
    using Web.Services.Helpers;

    public class AccountSerivce : IAccountSerivce
    {

        private readonly IHttpClientFactory httpClientFactory;

        public AccountSerivce(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<Result<TokenModel>> GetToken(LoginModel model)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"/login";
            HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl,model);
            if (!response.IsSuccessStatusCode)
            {
                  string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<TokenModel>(new ResultError(errorMessage));
            }
            ApiResponse? result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            TokenModel? token = JsonSerializer.Deserialize<TokenModel>(result?.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return new Result<TokenModel>(token);
        }
    }
}
