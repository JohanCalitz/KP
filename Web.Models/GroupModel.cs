namespace Web.Models
{
    using System;

    public class GroupModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PermissionModel> GroupPermissions { get; set; } = new ();

}
}
