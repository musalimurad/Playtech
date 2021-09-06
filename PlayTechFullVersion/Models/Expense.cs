using System;
using System.Collections.Generic;

#nullable disable

namespace PlayTechFullVersion.Models
{
    public partial class Expense
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public decimal FloorPrice { get; set; }
        public decimal Communal { get; set; }
        public decimal WorkerSalary { get; set; }
        public decimal AllMoney { get; set; }
        public decimal Price { get; set; }
        public DateTime CalcDate { get; set; }

    }
}
