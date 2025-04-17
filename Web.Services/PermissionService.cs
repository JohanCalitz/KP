namespace Web.Services
{
    using System.Net.Http.Json;
    using System.Text.Json;
    using Web.Models;
    using Web.Services.Helpers;
    using Web.Services.Interfaces;

    public class PermissionService : IPermissionService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public PermissionService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<Result<PagedResult<PermissionModel>>> GetPagedPermissionsAsync(int pageNumber = 1, int pageSize = 10)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"/api/Permission/paged?pageNumber={pageNumber}&pageSize={pageSize}";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                  string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<PagedResult<PermissionModel>>(new ResultError(errorMessage));
            }
            ApiResponse? result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (!string.IsNullOrEmpty(result?.ErrorMessage))
            {
                return new Result<PagedResult<PermissionModel>>(new ResultError(result.ErrorMessage));
            }
            PagedResult<PermissionModel>? pagedResult = JsonSerializer.Deserialize<PagedResult<PermissionModel>>(result?.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (pagedResult == null)
            {
                return new Result<PagedResult<PermissionModel>>(new ResultError("Failed to fetch paged permissions."));
            }

            return new Result<PagedResult<PermissionModel>>(pagedResult);
        }

    }
}
