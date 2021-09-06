using System;
using System.Collections.Generic;

#nullable disable

namespace PlayTechFullVersion.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? SaleDate { get; set; }
        public string OrderCode { get; set; }
        //public bool IsRefunded { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
