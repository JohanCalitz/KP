namespace API.Services.Interfaces
{
    using API.Models;
    using API.Services.Helpers;
    using System;
    using System.Threading.Tasks;

    public interface IGroupService
    {
        Task<Result<GroupModel>> CreateGroupAsync(GroupModel groupModel);
        Task<Result<GroupModel>> GetGroupByIdAsync(Guid groupId);
        Task<Result<List<GroupModel>>> GetGroupsUserDoesNotHaveAsync(Guid userId);
        Task<Result<List<GroupModel>>> GetGroupsUserDoesHaveAsync(Guid userId);
        Task<Result<PagedResult<GroupModel>>> GetAllGroupsAsync(int pageNumber = 1, int pageSize = 10);
        Task<Result<bool>> UpdateGroupAsync(Guid groupId, GroupModel groupModel);
        Task<Result<bool>> DeleteGroupAsync(Guid groupId);
        Task<Result<bool>> AddUserToGroupAsync(Guid groupId, Guid userId);
        Task<Result<bool>> RemoveUserFromGroupAsync(Guid groupId, Guid userId);
        Task<Result<bool>> AssignPermissionToGroupAsync(Guid groupId, Guid permissionId);
        Task<Result<bool>> RemovePermissionFromGroupAsync(Guid groupId, Guid permissionId);
    }
}
