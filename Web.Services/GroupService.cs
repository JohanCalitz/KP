namespace Web.Services
{
    using System;
    using System.Net.Http.Json;
    using System.Text.Json;
    using Web.Models;
    using Web.Services.Helpers;
    using Web.Services.Interfaces;

    public class GroupService: IGroupService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public GroupService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Result<bool>> AddUserToGroupAsync(Guid groupId, Guid userId)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"api/group/{groupId}/users/{userId}";

            var payload = new { groupId, userId };
            HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, payload);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<bool>(new ResultError(errorMessage));
            }

            ApiResponse? result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (!string.IsNullOrEmpty(result?.ErrorMessage))
            {
                return new Result<bool>(new ResultError(result.ErrorMessage));
            }

            return new Result<bool>(true);
        }

        public async Task<Result<bool>> RemoveUserFromGroupAsync(Guid groupId, Guid userId)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"api/group/{groupId}/users/{userId}";

            HttpResponseMessage response = await client.DeleteAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<bool>(new ResultError(errorMessage));
            }

            ApiResponse? result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (!string.IsNullOrEmpty(result?.ErrorMessage))
            {
                return new Result<bool>(new ResultError(result.ErrorMessage));
            }

            return new Result<bool>(true);
        }
        public async Task<Result<List<GroupModel>>> GetHasGroupsAsync(Guid userId)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"api/Group/HasGroup/{userId}";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<List<GroupModel>>(new ResultError(errorMessage));
            }
            ApiResponse? result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (!string.IsNullOrEmpty(result?.ErrorMessage))
            {
                return new Result<List<GroupModel>>(new ResultError(result.ErrorMessage));
            }
            var pagedResult = JsonSerializer.Deserialize<List<GroupModel>>(result?.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (pagedResult == null)
            {
                return new Result<List<GroupModel>>(new ResultError("Failed to fetch paged groups."));
            }

            return new Result<List<GroupModel>>(pagedResult);
        }
        public async Task<Result<List<GroupModel>>> GetHasNotGroupsAsync(Guid userId)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"api/Group/HasNotGroup/{userId}";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<List<GroupModel>>(new ResultError(errorMessage));
            }
            ApiResponse? result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (!string.IsNullOrEmpty(result?.ErrorMessage))
            {
                return new Result<List<GroupModel>>(new ResultError(result.ErrorMessage));
            }
            var pagedResult = JsonSerializer.Deserialize<List<GroupModel>>(result?.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (pagedResult == null)
            {
                return new Result<List<GroupModel>>(new ResultError("Failed to fetch paged groups."));
            }

            return new Result<List<GroupModel>>(pagedResult);
        }
        public async Task<Result<PagedResult<GroupModel>>> GetPagedGroupsAsync(int pageNumber = 1, int pageSize = 10)
        {
            using HttpClient client = httpClientFactory.CreateClient("ApiClient");
            string apiUrl = $"api/Group/paged?pageNumber={pageNumber}&pageSize={pageSize}";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                  string errorMessage = response.ReasonPhrase ?? "Unknown error occurred.";
                return new Result<PagedResult<GroupModel>>(new ResultError(errorMessage));
            }
            ApiResponse? result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (!string.IsNullOrEmpty(result?.ErrorMessage))
            {
                return new Result<PagedResult<GroupModel>>(new ResultError(result.ErrorMessage));
            }
            PagedResult<GroupModel>? pagedResult = JsonSerializer.Deserialize<PagedResult<GroupModel>>(result?.Data.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (pagedResult == null)
            {
                return new Result<PagedResult<GroupModel>>(new ResultError("Failed to fetch paged groups."));
            }

            return new Result<PagedResult<GroupModel>>(pagedResult);
        }
    }
}

