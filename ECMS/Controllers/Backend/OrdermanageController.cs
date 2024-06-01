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
    public class OrdermanageController : AdminBaseController
    {
        public ActionResult Index()
        {
            var ordermanages = _dbContext.Orders.OrderByDescending(i => i.id).ToList();
            var config = new MapperConfiguration(c => c.CreateMap<Order, OrderModel>());
            var mapper = new Mapper(config);
            var orders = mapper.Map<List<OrderModel>>(ordermanages);

            var ordertypes = _dbContext.OrderStatuses.ToList();
            ViewBag.OrderTypes = ordertypes;
            return View(orders);
        }

        [HttpPost]
        public ActionResult StatusChange()
        {
            var orderId = int.Parse(Request.Form["orderId"]);
            var status = int.Parse(Request.Form["status"]);

            var order = _dbContext.Orders.FirstOrDefault(i => i.id == orderId);
            order.orderstatus = status;
            _dbContext.SaveChanges();

            TempData["Success"] = "Order status changed successfully";
            return RedirectToAction("Index");
        }
    }
}