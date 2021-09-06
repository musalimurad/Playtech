using System;
using System.Collections.Generic;

#nullable disable

namespace PlayTechFullVersion.Models
{
    public partial class Firm
    {
        public Firm()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string FirmName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
