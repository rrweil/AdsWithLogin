using System;

namespace HW3._17._21AdsWithLogin.Data
{
    public class Ad
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public DateTime DateCreated { get; set;  }
        public string PhoneNumber { get; set; }

        public string PostedBy { get; set; }

        public int UserId { get; set; }
    }
    
}
