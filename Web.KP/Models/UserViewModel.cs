namespace Web.KP.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserViewModel
    {
        public Guid UserId { get; set; } = default!;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<GroupViewModel> HasGroup { get; set; } = new();
        public List<GroupViewModel> HasNotGroup { get; set; } = new();

    }
}
