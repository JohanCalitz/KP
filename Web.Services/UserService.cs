namespace Web.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Web.Models;
    using Web.Services.Helpers;
    using Web.Services.Interfaces;

    public class UserService : IUserService
    {

        private readonly IHttpClientFactory httpClientFactory;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Result<bool>> DeleteUserAsync(Guid id)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"/api/User/{id}";
            HttpResponseMessage response = await client.DeleteAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                  string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<bool>(new ResultError(errorMessage));
            }
            return new Result<bool>(true);
        }

        public async Task<Result<PagedResult<UserModel>>> GetPagedUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"/api/User/paged?pageNumber={pageNumber}&pageSize={pageSize}";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<PagedResult<UserModel>>(new ResultError(errorMessage));
            }
            ApiResponse? result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (!string.IsNullOrEmpty(result?.ErrorMessage))
            {
                return new Result<PagedResult<UserModel>>(new ResultError(result.ErrorMessage));
            }
            PagedResult<UserModel>? pagedResult = JsonSerializer.Deserialize<PagedResult<UserModel>>(result?.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });
            if (pagedResult == null)
            {
                return new Result<PagedResult<UserModel>>(new ResultError("Failed to fetch paged users."));
            }

            return new Result<PagedResult<UserModel>>(pagedResult);
        }

        public async Task<Result<UserModel>> GetUserByIdAsync(Guid id)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"/api/User/{id}";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<UserModel>(new ResultError(errorMessage));
            }

            ApiResponse? result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (result == null)
            {
                return new Result<UserModel>(new ResultError("User not found."));
            }
            UserModel? user = JsonSerializer.Deserialize<UserModel>(result?.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return new Result<UserModel>(user);
        }
        public async Task<Result<bool>> CreateUserAsync(UserModel userModel)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"/api/User";

            HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, userModel);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<bool>(new ResultError(errorMessage));
            }

            return new Result<bool>(true);
        }
        public async Task<Result<bool>> UpdateUserAsync(Guid id, UserModel userModel)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"/api/User/{id}";

            HttpResponseMessage response = await client.PutAsJsonAsync(apiUrl, userModel);

            if (!response.IsSuccessStatusCode)
            {
                  string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<bool>(new ResultError(errorMessage));
            }

            return new Result<bool>(true);
        }

        public async Task<Result<DashboardModel>> GetDashboardAsync()
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"/api/User/Dashboard";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<DashboardModel>(new ResultError(errorMessage));
            }

            ApiResponse? result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (result == null)
            {
                return new Result<DashboardModel>(new ResultError("User not found."));
            }
            DashboardModel? dashboard = JsonSerializer.Deserialize<DashboardModel>(result?.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return new Result<DashboardModel>(dashboard);
        }
    }
}
