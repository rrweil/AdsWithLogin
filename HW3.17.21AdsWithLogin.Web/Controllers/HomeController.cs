using HW3._17._21AdsWithLogin.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HW3._17._21AdsWithLogin.Data;
using Microsoft.AspNetCore.Authorization;

namespace HW3._17._21AdsWithLogin.Web.Controllers
{
    public class HomeController : Controller
    {

        private string _connectionString =
            "Data Source=.\\sqlexpress;Initial Catalog=AdsWithLogin;Integrated Security=true;";


        public IActionResult Index()
        {
            var db = new Db(_connectionString);
            var ads = db.GetAllAds();
            int userId = -1;
            if (User.Identity.IsAuthenticated)
            {
                userId = db.GetByEmail(User.Identity.Name).Id;
            }
            var vm = new IndexViewModel()
            {
                Ads = ads,
                AdIds = ads.Where(ad => ad.UserId == userId).Select(ad => ad.Id).ToList()
             };

            return View(vm);
        }


        [Authorize]
        public IActionResult NewAd()
        {
            var db = new Db(_connectionString);
            string email = User.Identity.Name;
            var vm = new NewAdViewModel()
            {
                CurrentUserEmail = db.GetByEmail(email).Email
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult NewAd(Ad ad, string email)
        {
            var db = new Db(_connectionString);
            ad.UserId = db.GetByEmail(User.Identity.Name).Id;
            db.NewAd(ad);
            var currentUser = db.GetByEmail(email);
            if (currentUser.AdIds == null)
            {
                currentUser.AdIds = new List<int>();
            }
            currentUser.AdIds.Add(ad.Id);

            return Redirect("/");
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            var db = new Db(_connectionString);
            var ads = db.GetAllAds();
            var userId = db.GetByEmail(User.Identity.Name).Id;
            var vm = new MyAccountViewModel()
            {
                Ads = ads.Where(ad => ad.UserId == userId).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult DeletePosting(int id)
        {
            var db = new Db(_connectionString);
            db.DeleteAd(id);
            return Redirect("/");
        }


    }
}
