namespace Web.KP.Models
{
    public class GroupViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public List<string> Permissions { get; set; } = new();
    }
}
