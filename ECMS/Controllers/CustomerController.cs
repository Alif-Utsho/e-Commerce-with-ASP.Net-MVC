using AutoMapper;
using ECMS.Auth;
using ECMS.Models.Database;
using ECMS.Models.Entities;
using ECMS.Models.RequestModel;
using ECMS.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ECMS.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(GlobalDataService globalDataService, DBEntities dbContext)
        : base(globalDataService, dbContext, new ShoppingCartService(System.Web.HttpContext.Current)) { }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["customerId"] != null)
            {
                return RedirectToAction("Index");
            }
            return View(new LoginRequestModel());
        }
        [HttpPost]
        public ActionResult Login(LoginRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Fill all the required fields";
                return View(model);
            }

            var user = _dbContext.Customers.FirstOrDefault(i => i.email.Equals(model.email));
            if (user != null)
            {
                var passwordHasher = new PasswordHasher();
                var verificationResult = passwordHasher.VerifyHashedPassword(user.password, model.password);
                if (verificationResult != PasswordVerificationResult.Success)
                {
                    TempData["Error"] = "Invalid Login Credential";
                    return View(model);
                }

                Session["customerId"] = user.id;
                Session["Customer"] = user;
                TempData["Success"] = "Logged in successfully";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Invalid username or password";
            return View(model);
        }

        public ActionResult Register()
        {
            return View(new CustomerModel());
        }

        [HttpPost]
        public ActionResult Register(CustomerModel customerModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Fill all the required fields";
                return View(customerModel);
            }

            var customer = _dbContext.Customers.FirstOrDefault(i => i.email.Equals(customerModel.email));
            if (customer == null)
            {
                var passwordHasher = new PasswordHasher();
                string hashedPassword = passwordHasher.HashPassword(customerModel.password);
                var customerDB = new Customer
                {
                    name = customerModel.name,
                    email = customerModel.email,
                    phone = customerModel.phone,
                    address = customerModel.address,
                    password = hashedPassword
                };
                _dbContext.Customers.Add(customerDB);
                _dbContext.SaveChanges();
                customer = customerDB;
            
                Session["customerId"] = customer.id;
                Session["Customer"] = customer;
                TempData["Success"] = "Registration successfull";
                return RedirectToAction("Index", "Customer");
            }

            TempData["Success"] = "E-Mail already exist in an account";
            return View(customerModel);

        }

        public ActionResult Logout()
        {
            Session.Remove("customerId");
            Session.Remove("Customer");
            TempData["Error"] = "Logged out";
            return RedirectToAction("Login", "Customer");
        }

        public ActionResult Index()
        {
            if (!CheckAuthenticate())
            {
                return RedirectToAction("Login", "Customer");
            }
            var customerId = int.Parse(Session["customerId"].ToString());

            var _orders = _dbContext.Orders.Where(i => i.customer_id == customerId).OrderByDescending(i => i.created_at).ToList();
            var config = new MapperConfiguration(c => c.CreateMap<Order, OrderModel>());
            var mapper = new Mapper(config);
            var orders = mapper.Map<List<OrderModel>>(_orders);
            
            ViewBag.Orders = orders;
            return View();
        }

        public ActionResult Profile()
        {
            if (!CheckAuthenticate())
            {
                return RedirectToAction("Login", "Customer");
            }
            var customerId = int.Parse(Session["customerId"].ToString());

            var user = _dbContext.Customers.FirstOrDefault(i => i.id == customerId);
            var config = new MapperConfiguration(c => c.CreateMap<Customer, CustomerModel>());
            var mapper = new Mapper(config);
            var customer = mapper.Map<CustomerModel>(user);

            return View(customer);
        }
        [HttpPost]
        public ActionResult ProfileUpdate(CustomerModel customerRequest)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Fill all the required fields";
                return RedirectToAction("Profile", customerRequest);
            }

            var customerId = int.Parse(Session["customerId"].ToString());
            var user = _dbContext.Customers.FirstOrDefault(i => i.id == customerId);
            user.name = customerRequest.name;
            user.address = customerRequest.address;
            user.phone = customerRequest.phone;
            _dbContext.SaveChanges();

            TempData["Success"] = "Profile Updated.";
            return RedirectToAction("Profile");
        }



        public bool CheckAuthenticate()
        {
            if (Session["customerId"] == null)
            {
                return false;
            }
            return true;
        }

    }
}