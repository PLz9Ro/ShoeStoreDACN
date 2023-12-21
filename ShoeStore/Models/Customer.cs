using System;
using System.Collections.Generic;

namespace ShoeStore.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string? FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? LocationId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Salt { get; set; }
        public DateTime LastLogin { get; set; }
        public bool Active { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Gender { get; set; }

        public virtual Location? Location { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
