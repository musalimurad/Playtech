using System;
using System.Collections.Generic;

#nullable disable

namespace WinFormsAppModel.Models
{
    public partial class SaleType
    {
        public SaleType()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
