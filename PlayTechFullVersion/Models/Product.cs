using System;
using System.Collections.Generic;

#nullable disable

namespace PlayTechFullVersion.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public int FirmId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? SalePrice { get; set; }
        public string BarCode { get; set; }

        public virtual Category Category { get; set; }
        public virtual Firm Firm { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
