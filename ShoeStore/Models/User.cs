using System;
using System.Collections.Generic;

namespace ShoeStore.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Password { get; set; } = null!;
        public string? EnCryPass { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool? Active { get; set; }
        public string? FullName { get; set; }
        public int? RoleId { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreateDay { get; set; }
        public string? ConfirmPassword { get; set; }

        public virtual Role? Role { get; set; }
    }
}
