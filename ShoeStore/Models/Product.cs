using System;
using System.Collections.Generic;

namespace ShoeStore.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ShortDesciption { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public int? Price { get; set; }
        public int? Discount { get; set; }
        public string? Thumb { get; set; }
        public string? ProductImage { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool? BestSellers { get; set; }
        public bool? HomeFlag { get; set; }
        public bool? Active { get; set; }
        public string? Tags { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? MetaDesc { get; set; }
        public string? MetaKey { get; set; }
        public int? UnitsStock { get; set; }
        public int? SizeId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Size? Size { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
