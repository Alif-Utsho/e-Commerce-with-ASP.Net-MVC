
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class AdminAccess : AuthorizeAttribute
{
    private readonly string[] allowedRoles;
    public AdminAccess(params string[] roles)
    {
        this.allowedRoles = roles;
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var role = httpContext.Session["Role"]?.ToString();
        return role != null && allowedRoles.Contains(role);
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new
        {
            controller = "Account",
            action = "Login"
        }));
    }
}