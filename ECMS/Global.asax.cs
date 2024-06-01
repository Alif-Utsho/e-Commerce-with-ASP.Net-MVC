using ECMS.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ECMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UnityConfig.RegisterComponents();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.IsInRole("Admin"))
                {
                    if (!HttpContext.Current.Request.Url.AbsolutePath.Contains("/Admin"))
                    {
                        Response.Redirect("~/Admin/Login");
                    }
                }
                else if (HttpContext.Current.User.IsInRole("Customer"))
                {
                    if (!HttpContext.Current.Request.Url.AbsolutePath.Contains("/Customer"))
                    {
                        Response.Redirect("~/Customer/Login");
                    }
                }
            }
        }
    }
}
