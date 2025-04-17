namespace API.Services.Interfaces
{
    using API.Models;
    using API.Services.Helpers;
    using System;
    using System.Threading.Tasks;

    public interface IPermissionService
    {
        Task<Result<PermissionModel>> CreatePermissionAsync(PermissionModel permissionModel);
        Task<Result<PermissionModel>> GetPermissionByIdAsync(Guid id);
        Task<Result<PagedResult<PermissionModel>>> GetAllPermissionsAsync(int pageNumber = 1, int pageSize = 10);
        Task<Result<bool>> UpdatePermissionAsync(Guid id, PermissionModel permissionModel);
        Task<Result<bool>> DeletePermissionAsync(Guid id);
    }
}
