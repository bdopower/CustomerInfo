using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerInfoApp.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace CustomerInfoApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministratorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public AdministratorController()
        {

        }
        public AdministratorController(ApplicationUserManager userManager)
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

        // GET: Administrator
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            List<UserModel> users = new List<UserModel>();
            foreach (var user in UserManager.Users.ToList())
            {
                users.Add(MapUserEntityToUserModel(user));
            }
            return View(users);
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

        // GET: Administrator/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entityUser = UserManager.FindByIdAsync(id).Result;
            UserModel userModel = MapUserEntityToUserModel(entityUser);

            if (userModel == null)
            {
                return HttpNotFound();
            }
            return View(userModel);
        }

        // GET: Administrator/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrator/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Role")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                //db.UserModels.Add(userModel);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userModel);
        }

        // GET: Administrator/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = UserManager.FindByIdAsync(id).Result;
            UserModel userModel = MapUserEntityToUserModel(entity);
            if (userModel == null)
            {
                return HttpNotFound();
            }
            return View(userModel);
        }

        // POST: Administrator/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Role")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(userModel).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userModel);
        }

        // GET: Administrator/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var entity = UserManager.FindByIdAsync(id).Result;
            UserModel userModel = MapUserEntityToUserModel(entity);
            if (userModel == null)
            {
                return HttpNotFound();
            }
            return View(userModel);
        }

        // POST: Administrator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var entity = UserManager.FindByIdAsync(id).Result;


            var userCustomers = db.Customers.Where(q => q.RegisteredById == entity.Id).ToList();
            var loggedInUser = UserManager.FindByNameAsync(User.Identity.Name).Result;
            foreach (var item in userCustomers)
            {
                item.RegisteredById = loggedInUser.Id;
            }
            db.SaveChanges();

            var userRole = UserManager.GetRolesAsync(entity.Id).Result.FirstOrDefault();
            if (userRole == null)
            {
                var results = await UserManager.DeleteAsync(entity);

                // If successful
                if (results.Succeeded)
                {
                    // Redirect to Users page
                    return RedirectToAction("Index");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                // Remove user from role first!
                var remFromRole = await UserManager.RemoveFromRoleAsync(id, userRole);
                // If successful
                if (remFromRole.Succeeded)
                {
                    // Remove user from UserStore
                    var results = await UserManager.DeleteAsync(entity);

                    // If successful
                    if (results.Succeeded)
                    {
                        // Redirect to Users page
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private UserModel MapUserEntityToUserModel(ApplicationUser entity)
        {
            return new UserModel()
            {
                Id = entity.Id,
                Role = UserManager.GetRolesAsync(entity.Id).Result.FirstOrDefault(),
                UserName = entity.UserName
            };
        }
    }
}
