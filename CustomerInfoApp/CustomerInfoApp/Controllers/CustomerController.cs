using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerInfoApp.DataEntities;
using CustomerInfoApp.Models;
using Microsoft.AspNet.Identity.Owin;

namespace CustomerInfoApp.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public CustomerController()
        {

        }
        public CustomerController(ApplicationUserManager userManager)
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

        public ActionResult List()
        {
            return View();
        }
        public JsonResult CustomerJson()
        {
            List<CustomerModel> dtoList = new List<CustomerModel>();
            if (User.IsInRole("Admin"))
            {
                foreach (var item in db.Customers.ToList())
                {
                    dtoList.Add(new CustomerModel()
                    {
                        ActiveStatus = item.isActive == true ? "Active" : "Passive",
                        Certificate = item.Certificate,
                        EndDate = item.EndDate.HasValue ? item.EndDate.Value.ToShortDateString() : string.Empty,
                        FullName = item.FullName,
                        Id = item.Id,
                        Phone = item.Phone,
                        Notes = item.Notes,
                        OtherPhone = item.OtherPhone,
                        RegisteredBy = item.RegisteredBy.UserName,
                        ReportNo = item.ReportNumber.ToString(),
                        StartDate = item.StartDate.ToShortDateString()
                    });
                }
            }
            else
            {
                var loggedInUser = UserManager.FindByNameAsync(User.Identity.Name).Result;
                foreach (var item in db.Customers.Where(q => q.RegisteredById == loggedInUser.Id).ToList())
                {
                    dtoList.Add(new CustomerModel()
                    {
                        ActiveStatus = item.isActive == true ? "Active" : "Passive",
                        Certificate = item.Certificate,
                        EndDate = item.EndDate.HasValue ? item.EndDate.Value.ToShortDateString() : string.Empty,
                        FullName = item.FullName,
                        Id = item.Id,
                        Phone = item.Phone,
                        Notes = item.Notes,
                        OtherPhone = item.OtherPhone,
                        RegisteredBy = item.RegisteredBy.UserName,
                        ReportNo = item.ReportNumber.ToString(),
                        StartDate = item.StartDate.ToShortDateString()
                    });
                }
            }


            return new JsonResult() { Data = dtoList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: Customer
        public ActionResult Index()
        {
            var user = UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (!user.isPasswordChanged)
                return RedirectToAction("ChangePassword", "Manage", null);
            var customers = db.Customers.Include(c => c.RegisteredBy);
            return RedirectToAction("List");
            //return View(customers.ToList());
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            ViewBag.RegisteredById = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,Phone,Certificate,ReportNumber,StartDate,EndDate,isActive,RegisteredById")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegisteredById = new SelectList(db.Users, "Id", "Email", customer.RegisteredById);
            return View(customer);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegisteredById = new SelectList(db.Users, "Id", "Email", customer.RegisteredById);
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,Phone,OtherPhone,Certificate,ReportNumber,StartDate,EndDate,isActive,RegisteredById,Notes")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            ViewBag.RegisteredById = new SelectList(db.Users, "Id", "Email", customer.RegisteredById);
            return View(customer);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
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
    }
}
