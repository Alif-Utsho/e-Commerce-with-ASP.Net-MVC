using ECMS.Models.Database;
using ECMS.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers.Backend
{
    public class CouponController : AdminBaseController
    {

        public ActionResult Index()
        {
            var coupons = _dbContext.CouponCodes.ToList();
            return View(coupons);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Upload(CouponRequestModel couponRequest)
        {
            var couopn = new CouponCode
            {
                coupon = couponRequest.coupon.ToUpper(),
                discount = couponRequest.discount,
                order_amount = couponRequest.order_amount,
                percentage = couponRequest.percentage,
                status = true
            };

            _dbContext.CouponCodes.Add(couopn);
            _dbContext.SaveChanges();

            TempData["Success"] = "Data Inserted successfully";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var coupon = _dbContext.CouponCodes.FirstOrDefault(i => i.id == id);
            return View(coupon);
        }

        public ActionResult Update(CouponRequestModel couponRequest)
        {
            var coupon = _dbContext.CouponCodes.FirstOrDefault(i => i.id == couponRequest.id);
            coupon.coupon = couponRequest.coupon.ToUpper();
            coupon.discount = couponRequest.discount;
            coupon.order_amount = couponRequest.order_amount;
            coupon.percentage = couponRequest.percentage;
            _dbContext.SaveChanges();

            TempData["Success"] = "Data Updated successfully";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var coupon = _dbContext.CouponCodes.FirstOrDefault(i => i.id == id);
            _dbContext.CouponCodes.Remove(coupon);
            _dbContext.SaveChanges();

            TempData["Success"] = "Data Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}