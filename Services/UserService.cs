namespace API.Services
{
    using API.Data.Models;
    using API.Data;
    using API.Models;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using API.Services.Helpers;
    using API.Services.Interfaces;

    public class UserService: IUserService
    {
        private readonly KingPriceDbContext context;
        private readonly IMapper mapper;

        public UserService(KingPriceDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<Result<UserModel>> CreateUserAsync(UserModel userModel)
        {
            try
            {
                User user = mapper.Map<User>(userModel);
                user.CreatedAt = DateTime.UtcNow;
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return new Result<UserModel>(mapper.Map<UserModel>(user));
            }
            catch (Exception)
            {
                return new Result<UserModel>(new ResultError($"Failed to create user"));
            }
        }
        public async Task<Result<DashboardModel>> GetDashboardAsync()
        {
            try
            {
                DashboardModel dashboardModel = new();
                dashboardModel.Users = await context.Users.CountAsync();
                dashboardModel.Groups = await context.Groups.CountAsync();
                dashboardModel.Permissions = await context.Permissions.CountAsync();
                return new Result<DashboardModel>(dashboardModel);
            }
            catch (Exception)
            {
                return new Result<DashboardModel>(new ResultError($"Failed to create user"));
            }
        }
        public async Task<Result<PagedResult<UserModel>>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                IQueryable<User> query = context.Users.AsQueryable();

                int totalItems = await query.CountAsync();

                List<User> users = await query
                    .OrderBy(u => u.FullName) // Optional sorting
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                List<UserModel> mappedUsers = mapper.Map<List<UserModel>>(users);

                PagedResult<UserModel> pagedResult = new()
                {
                    Items = mappedUsers,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return new Result<PagedResult<UserModel>>(pagedResult);
            }
            catch (Exception)
            {
                return new Result<PagedResult<UserModel>>(new ResultError("Failed to retrieve users"));
            }
        }
        public async Task<Result<UserModel>> GetUserByIdAsync(Guid id)
        {
            try
            {
                User? user = await context.Users
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return new Result<UserModel>(new ResultError("User not found"));
                }

                return new Result<UserModel>(mapper.Map<UserModel>(user));
            }
            catch (Exception)
            {
                return new Result<UserModel>(new ResultError($"Failed to retrieve user"));
            }
        }

        public async Task<Result<UserAllInformationModel>> GetUserAllInformationAsync(Guid id)
        {
            try
            {
                User? user = await context.Users
                    .Include(u => u.UserGroups)
                    .ThenInclude(ug => ug.Group)
                    .ThenInclude(g => g.GroupPermissions)
                    .ThenInclude(gp => gp.Permission)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return new Result<UserAllInformationModel>(new ResultError("User not found"));
                }

                UserAllInformationModel userAllInfo = new()
                {
                    User = mapper.Map<UserModel>(user),
                    Groups = user.UserGroups.Select(ug => mapper.Map<GroupModel>(ug.Group)).ToList()
                };

                return new Result<UserAllInformationModel>(userAllInfo);
            }
            catch (Exception)
            {
                return new Result<UserAllInformationModel>(new ResultError($"Failed to retrieve user information"));
            }
        }

        public async Task<Result<bool>> UpdateUserAsync(Guid id, UserModel userModel)
        {
            try
            {
                if (id != userModel.Id)
                {
                    return new Result<bool>(new ResultError("User ID mismatch"));
                }

                User? user = await context.Users.FindAsync(id);
                if (user == null)
                {
                    return new Result<bool>(new ResultError("User not found"));
                }

                mapper.Map(userModel, user);
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                return new Result<bool>(new ResultError($"Failed to update user"));
            }
        }

        public async Task<Result<bool>> DeleteUserAsync(Guid id)
        {
            try
            {
                User? user = await context.Users.FindAsync(id);
                if (user == null)
                {
                    return new Result<bool>(new ResultError("User not found"));
                }

                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                return new Result<bool>(new ResultError($"Failed to delete user"));
            }
        }

        public async Task<Result<bool>> AssignUserToGroupAsync(Guid userId, Guid groupId)
        {
            try
            {
                User? user = await context.Users.FindAsync(userId);
                Group? group = await context.Groups.FindAsync(groupId);

                if (user == null || group == null)
                {
                    return new Result<bool>(new ResultError("User or Group not found"));
                }

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
                return new Result<bool>(new ResultError($"Failed to assign user to group"));
            }
        }

        public async Task<Result<bool>> RemoveUserFromGroupAsync(Guid userId, Guid groupId)
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
    }
}
