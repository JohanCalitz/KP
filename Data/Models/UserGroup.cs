namespace API.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserGroup
    {
        [Key]
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = default!;
    }
}
