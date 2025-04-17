namespace API.Services
{
    using API.Data;
    using API.Data.Models;
    using API.Models;
    using API.Services.Helpers;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using API.Services.Interfaces;
    using System.Collections.Generic;

    public class GroupService: IGroupService
    {
        private readonly KingPriceDbContext context;
        private readonly IMapper mapper;

        public GroupService(KingPriceDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Result<GroupModel>> CreateGroupAsync(GroupModel groupModel)
        {
            try
            {
                Group group = mapper.Map<Group>(groupModel);
                context.Groups.Add(group);
                await context.SaveChangesAsync();
                return new Result<GroupModel>(mapper.Map<GroupModel>(group));
            }
            catch (Exception)
            {
                return new Result<GroupModel>(new ResultError($"Failed to create group"));
            }
        }
        public async Task<Result<GroupModel>> GetGroupByIdAsync(Guid id)
        {
            try
            {
                Group? group = await context.Groups
                    .Include(g => g.UserGroups)
                    .ThenInclude(ug => ug.User)
                    .Include(g => g.GroupPermissions)
                    .ThenInclude(gp => gp.Permission)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (group == null)
                {
                    return new Result<GroupModel>(new ResultError("Group not found"));
                }

                return new Result<GroupModel>(mapper.Map<GroupModel>(group));
            }
            catch (Exception)
            {
                return new Result<GroupModel>(new ResultError($"Failed to retrieve group"));
            }
        }
        public async Task<Result<List<GroupModel>>> GetGroupsUserDoesHaveAsync(Guid userId)
        {
            try
            {
                var userGroupIds = await context.UserGroups
                    .Where(ug => ug.UserId == userId)
                    .Select(ug => ug.GroupId)
                    .ToListAsync();

                var groups = await context.Groups
                    .Where(g => userGroupIds.Contains(g.Id))
                    .ToListAsync();

                return new Result<List<GroupModel>>(mapper.Map<List<GroupModel>>(groups));
            }
            catch (Exception)
            {
                return new Result<List<GroupModel>>(new ResultError("Failed to retrieve groups"));
            }
        }
        public async Task<Result<List<GroupModel>>> GetGroupsUserDoesNotHaveAsync(Guid userId)
        {
            try
            {
                var userGroupIds = await context.UserGroups
                    .Where(ug => ug.UserId == userId)
                    .Select(ug => ug.GroupId)
                    .ToListAsync();

                var groups = await context.Groups
                    .Where(g => !userGroupIds.Contains(g.Id))
                    .ToListAsync();

                if (groups == null || !groups.Any())
                {
                    return new Result<List<GroupModel>>(new ResultError("No groups available for the user"));
                }

                return new Result<List<GroupModel>>(mapper.Map<List<GroupModel>>(groups));
            }
            catch (Exception)
            {
                return new Result<List<GroupModel>>(new ResultError("Failed to retrieve groups"));
            }
        }
        public async Task<Result<PagedResult<GroupModel>>> GetAllGroupsAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IQueryable<Group> query = context.Groups.Include(x=>x.GroupPermissions)
                    .ThenInclude(x=>x.Permission).AsQueryable();

                int totalItems = await query.CountAsync();

                List<Group> groups = await query
                    .OrderBy(u => u.Name) // Optional sorting
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                List<GroupModel> mappedGroups = mapper.Map<List<GroupModel>>(groups);

                PagedResult<GroupModel> pagedResult = new()
                {
                    Items = mappedGroups,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return new Result<PagedResult<GroupModel>>(pagedResult);
            }
            catch (Exception)
            {
                return new Result<PagedResult<GroupModel>>(new ResultError("Failed to retrieve groups"));
            }
        }
        public async Task<Result<bool>> UpdateGroupAsync(Guid id, GroupModel groupModel)
        {
            try
            {
                if (id != groupModel.Id)
                {
                    return new Result<bool>(new ResultError("Group ID mismatch"));
                }

                Group? group = await context.Groups.FindAsync(id);
                if (group == null)
                {
                    return new Result<bool>(new ResultError("Group not found"));
                }

                mapper.Map(groupModel, group);
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                return new Result<bool>(new ResultError($"Failed to update group"));
            }
        }
        public async Task<Result<bool>> DeleteGroupAsync(Guid id)
        {
            try
            {
                Group? group = await context.Groups.FindAsync(id);
                if (group == null)
                {
                    return new Result<bool>(new ResultError("Group not found"));
                }

                context.Groups.Remove(group);
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                return new Result<bool>(new ResultError($"Failed to delete group"));
            }
        }
        public async Task<Result<bool>> AddUserToGroupAsync(Guid groupId,Guid userId)
        {
            try
            {
                User? user = await context.Users.FindAsync(userId);
                Group? group = await context.Groups.FindAsync(groupId);

                if (user == null || group == null)
                {
                    return new Result<bool>(new ResultError("User or Group not found"));
                }

                // Check if the user is already in the group
                if (context.UserGroups.Any(ug => ug.UserId == userId && ug.GroupId == groupId))
                {
                    return new Result<bool>(new ResultError("User is already in the group"));
                }

                context.UserGroups.Add(new UserGroup { UserId = userId, GroupId = groupId });
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                return new Result<bool>(new ResultError($"Failed to add user to group"));
            }
        }
        public async Task<Result<bool>> RemoveUserFromGroupAsync(Guid groupId, Guid userId)
        {
            try
            {
                UserGroup? userGroup = await context.UserGroups
                    .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId);

                if (userGroup == null)
                {
                    return new Result<bool>(new ResultError("User not found in the group"));
                }

                context.UserGroups.Remove(userGroup);
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                return new Result<bool>(new ResultError($"Failed to remove user from group"));
            }
        }
        public async Task<Result<bool>> AssignPermissionToGroupAsync(Guid groupId, Guid permissionId)
        {
            try
            {
                Group? group = await context.Groups.FindAsync(groupId);
                Permission? permission = await context.Permissions.FindAsync(permissionId);

                if (group == null || permission == null)
                {
                    return new Result<bool>(new ResultError("Group or Permission not found"));
                }

                // Check if the permission is already assigned to the group
                if (context.GroupPermissions.Any(gp => gp.GroupId == groupId && gp.PermissionId == permissionId))
                {
                    return new Result<bool>(new ResultError("Permission is already assigned to the group"));
                }

                context.GroupPermissions.Add(new GroupPermission { GroupId = groupId, PermissionId = permissionId });
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                return new Result<bool>(new ResultError($"Failed to assign permission to group"));
            }
        }
        public async Task<Result<bool>> RemovePermissionFromGroupAsync(Guid groupId, Guid permissionId)
        {
            try
            {
                GroupPermission? groupPermission = await context.GroupPermissions
                    .FirstOrDefaultAsync(gp => gp.GroupId == groupId && gp.PermissionId == permissionId);

                if (groupPermission == null)
                {
                    return new Result<bool>(new ResultError("Permission not found in the group"));
                }

                context.GroupPermissions.Remove(groupPermission);
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                return new Result<bool>(new ResultError($"Failed to remove permission from group"));
            }
        }
    }
}
