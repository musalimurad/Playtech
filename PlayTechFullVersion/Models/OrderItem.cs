using System;
using System.Collections.Generic;

#nullable disable

namespace WinFormsAppModel.Models
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int? Quantity { get; set; }
        public decimal? ItemPrice { get; set; }
        public DateTime? DailySaleDate { get; set; }
        public int? ProductId { get; set; }
        public int? OrderId { get; set; }
        public bool IsRefunded { get; set; }
        public int? RefundedCount { get; set; }
        public int? CreditMonthId { get; set; }
        public int? SaleTypeId { get; set; }

        public virtual CreditMonth CreditMonth { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual SaleType SaleType { get; set; }
    }
}
