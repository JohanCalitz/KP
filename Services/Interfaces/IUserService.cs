namespace API.Services.Interfaces
{
    using API.Models;
    using API.Services.Helpers;
    using System;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<Result<UserModel>> CreateUserAsync(UserModel userModel);
        Task<Result<UserModel>> GetUserByIdAsync(Guid id);
        Task<Result<DashboardModel>> GetDashboardAsync();
        Task<Result<PagedResult<UserModel>>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10);
        Task<Result<UserAllInformationModel>> GetUserAllInformationAsync(Guid id);
        Task<Result<bool>> UpdateUserAsync(Guid id, UserModel userModel);
        Task<Result<bool>> DeleteUserAsync(Guid id);
        Task<Result<bool>> AssignUserToGroupAsync(Guid userId, Guid groupId);
        Task<Result<bool>> RemoveUserFromGroupAsync(Guid userId, Guid groupId);
    }
}
