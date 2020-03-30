using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Comision.Helper.Filters
{
    [AttributeUsage(AttributeTargets.All | AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public  class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
                return false;
            else return true;

        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult(403);
                filterContext.Result = new ViewResult
                {
                    ViewName = "NotAuthorize"
                };
                return;
            }
            base.HandleUnauthorizedRequest(filterContext);
        }

    }
}
