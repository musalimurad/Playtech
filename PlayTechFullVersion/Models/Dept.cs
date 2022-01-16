using System;
using System.Collections.Generic;

#nullable disable

namespace WinFormsAppModel.Models
{
    public partial class Dept
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public decimal? TakeMoney { get; set; }
        public string TakeWhy { get; set; }
        public DateTime? TakeDate { get; set; }
    }
}
