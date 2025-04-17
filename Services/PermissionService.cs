namespace API.Services
{
    using API.Data;
    using API.Data.Models;
    using API.Models;
    using API.Services.Helpers;
    using API.Services.Interfaces;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public class PermissionService: IPermissionService
    {
        private readonly KingPriceDbContext context;
        private readonly IMapper mapper;

        public PermissionService(KingPriceDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Result<PermissionModel>> CreatePermissionAsync(PermissionModel permissionModel)
        {
            try
            {
                Permission permission = mapper.Map<Permission>(permissionModel);
                context.Permissions.Add(permission);
                await context.SaveChangesAsync();
                return new Result<PermissionModel>(mapper.Map<PermissionModel>(permission));
            }
            catch (Exception)
            {
                return new Result<PermissionModel>(new ResultError($"Failed to create permission"));
            }
        }

        public async Task<Result<PermissionModel>> GetPermissionByIdAsync(Guid id)
        {
            try
            {
                Permission? permission = await context.Permissions.FindAsync(id);
                if (permission == null)
                {
                    return new Result<PermissionModel>(new ResultError("Permission not found"));
                }

                return new Result<PermissionModel>(mapper.Map<PermissionModel>(permission));
            }
            catch (Exception)
            {
                return new Result<PermissionModel>(new ResultError($"Failed to retrieve permission"));
            }
        }
        public async Task<Result<PagedResult<PermissionModel>>> GetAllPermissionsAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IQueryable<Permission> query = context.Permissions.AsQueryable();

                int totalItems = await query.CountAsync();

                List<Permission> groups = await query
                    .OrderBy(u => u.Name) // Optional sorting
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                List<PermissionModel> mappedGroups = mapper.Map<List<PermissionModel>>(groups);

                PagedResult<PermissionModel> pagedResult = new()
                {
                    Items = mappedGroups,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return new Result<PagedResult<PermissionModel>>(pagedResult);
            }
            catch (Exception)
            {
                return new Result<PagedResult<PermissionModel>>(new ResultError("Failed to retrieve permission"));
            }
        }
        public async Task<Result<bool>> UpdatePermissionAsync(Guid id, PermissionModel permissionModel)
        {
            try
            {
                if (id != permissionModel.Id)
                {
                    return new Result<bool>(new ResultError("Permission ID mismatch"));
                }

                Permission? permission = await context.Permissions.FindAsync(id);
                if (permission == null)
                {
                    return new Result<bool>(new ResultError("Permission not found"));
                }

                mapper.Map(permissionModel, permission);
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                return new Result<bool>(new ResultError($"Failed to update permission"));
            }
        }

        public async Task<Result<bool>> DeletePermissionAsync(Guid id)
        {
            try
            {
                Permission? permission = await context.Permissions.FindAsync(id);
                if (permission == null)
                {
                    return new Result<bool>(new ResultError("Permission not found"));
                }

                context.Permissions.Remove(permission);
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                return new Result<bool>(new ResultError($"Failed to delete permission"));
            }
        }
    }
}
