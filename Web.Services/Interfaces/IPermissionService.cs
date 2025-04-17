namespace Web.Services.Interfaces
{
    using Web.Models;
    using Web.Services.Helpers;

    public interface IPermissionService
    {
        Task<Result<PagedResult<PermissionModel>>> GetPagedPermissionsAsync(int pageNumber = 1, int pageSize = 10);
    }
}
