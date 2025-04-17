namespace Web.Services.Interfaces
{
    using System.Threading.Tasks;
    using Web.Models;
    using Web.Services.Helpers;

    public interface IUserService
    {
        Task<Result<PagedResult<UserModel>>> GetPagedUsersAsync(int pageNumber = 1, int pageSize = 10);
        Task<Result<bool>> DeleteUserAsync(Guid id);
        Task<Result<bool>> UpdateUserAsync(Guid id, UserModel userModel);
        Task<Result<bool>> CreateUserAsync(UserModel userModel);
        Task<Result<UserModel>> GetUserByIdAsync(Guid id);
        Task<Result<DashboardModel>> GetDashboardAsync();
    }
}
