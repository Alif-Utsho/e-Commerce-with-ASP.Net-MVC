using ECMS.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers.Backend
{
    public class AdminBaseController : Controller
    {
        protected readonly DBEntities _dbContext;

        public AdminBaseController()
        {
            _dbContext = new DBEntities();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["adminId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Admin", action = "Login" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}