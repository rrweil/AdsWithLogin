using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HW3._17._21AdsWithLogin.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace HW3._17._21AdsWithLogin.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString =
           "Data Source=.\\sqlexpress;Initial Catalog=AdsWithLogin;Integrated Security=true;";


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var db = new Db(_connectionString);
            db.CreateNewAccount(user, password);
            return Redirect("home/login");
        }

        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var db = new Db(_connectionString);
            var user = db.Login(email, password);
            if (user == null)
            {
                TempData["message"] = "Invalid username/password combination. Try Again";
                return Redirect("/account/login");
            }

            var claims = new List<Claim>
            {
                new Claim("user", email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/newad");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
