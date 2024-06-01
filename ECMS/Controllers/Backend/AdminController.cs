using AutoMapper;
using ECMS.Models.Database;
using ECMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers.Backend
{
    public class AdminController : Controller
    {
        protected readonly DBEntities _dbContext;
        public AdminController()
        {
            _dbContext = new DBEntities();
        }
        public ActionResult Index()
        {
            if(Session["adminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var revenue = _dbContext.Products.Select(i => (i.new_price - i.purchase_price) * i.sold).Sum();
            var customerCount = _dbContext.Customers.Count();
            var orderCount = _dbContext.Orders.Count();
            ViewBag.Revenue = revenue;
            ViewBag.CustomerCount = customerCount;
            ViewBag.orderCount = orderCount;

            var ordermanages = _dbContext.Orders.OrderByDescending(i => i.id).Take(10).ToList();
            var config = new MapperConfiguration(c => c.CreateMap<Order, OrderModel>());
            var mapper = new Mapper(config);
            var orders = mapper.Map<List<OrderModel>>(ordermanages);
            ViewBag.Orders = orders;

            var customerList = _dbContext.Customers.OrderByDescending(i => i.id).Take(10).ToList();
            var config_customer = new MapperConfiguration(c => c.CreateMap<Customer, CustomerModel>());
            var mapper_customer = new Mapper(config_customer);
            var customers = mapper_customer.Map<List<CustomerModel>>(customerList);
            ViewBag.Customers = customers;

            return View();
        }
        public ActionResult Theme()
        {
            return View();
        }

        public ActionResult Users()
        {
            if (Session["adminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var users = _dbContext.Users.ToList();
            return View(users);
        }

        public ActionResult Create()
        {
            if (Session["adminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        public ActionResult Upload(User user)
        {
            if (Session["adminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            TempData["Success"] = "Data inserted successfully";
            return RedirectToAction("Users");
        }

        public ActionResult Edit(int id)
        {
            if (Session["adminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var user = _dbContext.Users.FirstOrDefault(i => i.id == id);
            return View(user);
        }

        public ActionResult Update(User user)
        {
            if (Session["adminId"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var db_user = _dbContext.Users.FirstOrDefault(i => i.id == user.id);
            db_user.name = user.name;
            db_user.email = user.email;
            db_user.password = user.password;
            _dbContext.SaveChanges();

            TempData["Success"] = "Data updated successfully";
            return RedirectToAction("Users");
        }

        public ActionResult Login()
        {
            if (Session["adminId"] != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult LoginSubmit()
        {
            var email = Request.Form["email"];
            var password = Request.Form["password"];

            var user = _dbContext.Users.FirstOrDefault(i => i.email.Equals(email) && i.password.Equals(password));
            if (user != null)
            {
                Session["adminId"] = user.id;

                TempData["Success"] = "Logged in successfully";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Invalid Credentials";
            return RedirectToAction("Login"); 
        }

        public ActionResult Logout()
        {
            Session.Remove("adminId");
            TempData["Error"] = "Logged out";
            return RedirectToAction("Login", "Admin");
        }
    }
}