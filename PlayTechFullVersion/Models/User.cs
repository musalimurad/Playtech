using System;
using System.Collections.Generic;

#nullable disable

namespace PlayTechFullVersion.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
