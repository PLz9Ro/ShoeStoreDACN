using System;
using System.Collections.Generic;

namespace ShoeStore.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public int? TransactStatusId { get; set; }
        public bool? Deleted { get; set; }
        public bool? Paid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? PaymentId { get; set; }
        public string? Note { get; set; }
        public string? Address { get; set; }
        public int? TotalMoney { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual TransactionStatus? TransactStatus { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
