namespace API.Services.Mapper
{
    using AutoMapper;
    using Data.Models;
    using Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<Group, GroupModel>()
                .ForMember(dest => dest.GroupPermissions, opt => opt.MapFrom(src => src.GroupPermissions.Select(p => new PermissionModel
                {
                    Id = p.Permission.Id,
                    Name = p.Permission.Name,
                    Description = p.Permission.Description
                }).ToList()));

            CreateMap<Permission, PermissionModel>();

            CreateMap<UserModel, User>();
            CreateMap<GroupModel, Group>()
                .ForMember(dest => dest.GroupPermissions, opt => opt.MapFrom(src => src.GroupPermissions.Select(p => new Permission
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                }).ToList()));

            CreateMap<PermissionModel, Permission>();
        }
    }
}
