using CustomerInfoApp.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerInfoApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager;
        public HomeController()
        {

        }
        public HomeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [Authorize]
        public ActionResult Index()
        {
            var user = UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (!user.isPasswordChanged)
                return RedirectToAction("ChangePassword", "Manage", null);
            return RedirectToAction("List", "Customer", null);
            //return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddUser()
        {
            ViewBag.Message = "Adding user page.";
            return RedirectToAction("Register", "Account", null);
            //return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}