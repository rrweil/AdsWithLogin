using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HW3._17._21AdsWithLogin.Data;

namespace HW3._17._21AdsWithLogin.Web.Models
{
    public class IndexViewModel
    {
        public List<Ad> Ads { get; set; }

        public List<int> AdIds { get; set; }

    }
}
