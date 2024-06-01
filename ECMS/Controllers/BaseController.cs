using ECMS.Models.Database;
using ECMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers
{
    public class BaseController : Controller
    {
        protected GlobalDataService _globalDataService;
        protected ShoppingCartService _shoppingCartService;
        protected readonly DBEntities _dbContext;

        public BaseController(GlobalDataService globalDataService, DBEntities dbContext, ShoppingCartService shoppingCartService)
        {
            _globalDataService = globalDataService;
            _dbContext = dbContext;
            _shoppingCartService = shoppingCartService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.SiteTitle = _globalDataService.SiteTitle;
            ViewBag.Logo = _globalDataService.Logo;
            ViewBag.Categories = _globalDataService.CategoryList;

            ViewBag.CartItems = _globalDataService.CartItems;
            ViewBag.CartTotal = _globalDataService.CartTotal;
            ViewBag.DiscountAmount = _globalDataService.DiscountAmount;
            ViewBag.DiscountCoupon = _globalDataService.DiscountCoupon;
            ViewBag.Customer = Session["Customer"];
        }
    }
}