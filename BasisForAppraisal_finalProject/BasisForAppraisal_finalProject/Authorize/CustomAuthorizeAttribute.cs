using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BasisForAppraisal_finalProject.Authorize
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string Roles { get; set; }

        public string[] RolesArray { get; set; }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new
                      RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }



            if ( RolesArray != null)
            {
                if (!RolesArray.Any(x => filterContext.HttpContext.User.IsInRole(x)))
                {
                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
                return;
            }

            if (!String.IsNullOrEmpty(Roles))
            {
                var r = filterContext.HttpContext.User.Identity.AuthenticationType;

                //var b = UserRoleDictionary.UserRole.Keys.Where(x => filterContext.HttpContext.User.Identity.Name == x);
                if (!filterContext.HttpContext.User.IsInRole(Roles))
                {

                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "Login" }));

                    // base.OnAuthorization(filterContext); //returns to login url
                }
            }


        }
    }
}