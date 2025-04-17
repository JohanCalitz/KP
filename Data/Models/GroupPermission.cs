namespace API.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GroupPermission
    {
        [Key]
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = default!;
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = default!;
    }
}
