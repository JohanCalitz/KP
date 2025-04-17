namespace API.KP.Controllers
{
    using API.KP.Common;
    using API.Models;
    using API.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using API.Services.Helpers;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : Controller
    {
        private readonly IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] GroupModel groupModel)
        {
            Result<GroupModel> result = await groupService.CreateGroupAsync(groupModel);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(Guid id)
        {
            Result<GroupModel> result = await groupService.GetGroupByIdAsync(id);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }
        [HttpGet("HasNotGroup/{id}")]
        public async Task<IActionResult> GetGroupsUserDoesNotHave(Guid id)
        {
            var result = await groupService.GetGroupsUserDoesNotHaveAsync(id);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }

        [HttpGet("HasGroup/{id}")]
        public async Task<IActionResult> GetGroupsUserDoesHave(Guid id)
        {
            var result = await groupService.GetGroupsUserDoesHaveAsync(id);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedGroups(int pageNumber = 1, int pageSize = 10)
        {
            Result<PagedResult<GroupModel>> result = await groupService.GetAllGroupsAsync(pageNumber, pageSize);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);

            }
            return new ApiResponse(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(Guid id, [FromBody] GroupModel groupModel)
        {
            Result<bool> result = await groupService.UpdateGroupAsync(id, groupModel);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            Result<bool> result = await groupService.DeleteGroupAsync(id);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }

        [HttpPost("{groupId}/users/{userId}")]
        public async Task<IActionResult> AddUserToGroup(Guid groupId, Guid userId)
        {
            Result<bool> result = await groupService.AddUserToGroupAsync(groupId, userId);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }

        [HttpDelete("{groupId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserFromGroup(Guid groupId, Guid userId)
        {
            Result<bool> result = await groupService.RemoveUserFromGroupAsync(groupId, userId);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }

        [HttpPost("{groupId}/permissions/{permissionId}")]
        public async Task<IActionResult> AssignPermissionToGroup(Guid groupId, Guid permissionId)
        {
            Result<bool> result = await groupService.AssignPermissionToGroupAsync(groupId, permissionId);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }

        [HttpDelete("{groupId}/permissions/{permissionId}")]
        public async Task<IActionResult> RemovePermissionFromGroup(Guid groupId, Guid permissionId)
        {
            Result<bool> result = await groupService.RemovePermissionFromGroupAsync(groupId, permissionId);
            if (result.State == ResultStateEnum.Faulted)
            {
                new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }
    }
}
