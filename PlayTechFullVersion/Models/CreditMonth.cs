using System;
using System.Collections.Generic;

#nullable disable

namespace WinFormsAppModel.Models
{
    public partial class CreditMonth
    {
        public CreditMonth()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string Month { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
