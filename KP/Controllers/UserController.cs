namespace API.KP.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using API.Models;
    using API.Services.Helpers;
    using API.KP.Common;
    using API.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserModel userModel)
        {
            Result<UserModel> result = await userService.CreateUserAsync(userModel);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }

            return new ApiResponse(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            Result<UserModel> result = await userService.GetUserByIdAsync(id);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }
            return new ApiResponse(result.Value);
        }
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await userService.GetDashboardAsync();
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);

            }
            return new ApiResponse(result.Value);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedUsers(int pageNumber = 1, int pageSize = 10)
        {
            Result<PagedResult<UserModel>> result = await userService.GetAllUsersAsync(pageNumber, pageSize);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);

            }
            return new ApiResponse(result.Value);
        }

        [HttpGet("{id}/all")]
        public async Task<IActionResult> GetUserAllInformation(Guid id)
        {
            Result<UserAllInformationModel> result = await userService.GetUserAllInformationAsync(id);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }

            return new ApiResponse(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserModel userModel)
        {
            Result<bool> result = await userService.UpdateUserAsync(id, userModel);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }

            return new ApiResponse(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            Result<bool> result = await userService.DeleteUserAsync(id);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }

            return new ApiResponse(result.Value);
        }

        [HttpPost("{userId}/groups/{groupId}")]
        public async Task<IActionResult> AssignUserToGroup(Guid userId, Guid groupId)
        {
            Result<bool> result = await userService.AssignUserToGroupAsync(userId, groupId);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }

            return new ApiResponse(result.Value);
        }

        [HttpDelete("{userId}/groups/{groupId}")]
        public async Task<IActionResult> RemoveUserFromGroup(Guid userId, Guid groupId)
        {
            Result<bool> result = await userService.RemoveUserFromGroupAsync(userId, groupId);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }

            return new ApiResponse(result.Value);
        }
    }
}

