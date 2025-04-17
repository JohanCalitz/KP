namespace API.Models
{
    using System;
    using System.Collections.Generic;

    public class GroupModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PermissionModel> GroupPermissions { get; set; } = new List<PermissionModel>();

    }
}
