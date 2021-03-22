using System;
using System.Collections.Generic;
using System.Text;

namespace HW3._17._21AdsWithLogin.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set;  }
        public List<int> AdIds { get; set; }
    }
}
