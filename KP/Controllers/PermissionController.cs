namespace API.KP.Controllers
{
    using API.KP.Common;
    using API.Models;
    using API.Services.Helpers;
    using API.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            this.permissionService = permissionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionModel permissionModel)
        {
            Result<PermissionModel> result = await permissionService.CreatePermissionAsync(permissionModel);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }

            return new ApiResponse(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermissionById(Guid id)
        {
            Result<PermissionModel> result = await permissionService.GetPermissionByIdAsync(id);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }

            return new ApiResponse(result.Value);
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedPermissions(int pageNumber = 1, int pageSize = 10)
        {
            Result<PagedResult<PermissionModel>> result = await permissionService.GetAllPermissionsAsync(pageNumber, pageSize);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);

            }
            return new ApiResponse(result.Value);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] PermissionModel permissionModel)
        {
            Result<bool> result = await permissionService.UpdatePermissionAsync(id, permissionModel);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }

            return new ApiResponse(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(Guid id)
        {
            Result<bool> result = await permissionService.DeletePermissionAsync(id);
            if (result.State == ResultStateEnum.Faulted)
            {
                return new ApiResponse(null, result.resulterror?.Message);
            }

            return new ApiResponse(result.Value);
        }
    }
}
