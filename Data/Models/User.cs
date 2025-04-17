namespace API.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = default!;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = default!;

        [Required]
        [MinLength(8)]
        [MaxLength(300)]
        public string Password { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    }
}