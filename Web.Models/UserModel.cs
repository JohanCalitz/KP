namespace Web.Models
{
    public class UserModel
    {
        public Guid Id { get; set; } = default!;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
