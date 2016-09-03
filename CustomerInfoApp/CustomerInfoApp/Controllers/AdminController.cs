using CustomerInfoApp.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerInfoApp.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public AdminController()
        {

        }
        public AdminController(ApplicationUserManager userManager)
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

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult UsersJson()
        {
            List<UserModel> users = new List<UserModel>();
            foreach (var user in UserManager.Users.ToList())
            {
                users.Add(new UserModel()
                {
                    Id = user.Id,
                    Role = UserManager.GetRolesAsync(user.Id).Result.FirstOrDefault(),
                    UserName = user.UserName
                });
            }
            return new JsonResult() { Data = users, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}