namespace Web.Services.Interfaces
{
    using Web.Models;
    using Web.Services.Helpers;

    public interface IGroupService
    {
        Task<Result<bool>> AddUserToGroupAsync(Guid groupId, Guid userId);
        Task<Result<bool>> RemoveUserFromGroupAsync(Guid groupId, Guid userId);
        Task<Result<PagedResult<GroupModel>>> GetPagedGroupsAsync(int pageNumber = 1, int pageSize = 10);
        Task<Result<List<GroupModel>>> GetHasNotGroupsAsync(Guid userId);
        Task<Result<List<GroupModel>>> GetHasGroupsAsync(Guid userId);
    }
}
