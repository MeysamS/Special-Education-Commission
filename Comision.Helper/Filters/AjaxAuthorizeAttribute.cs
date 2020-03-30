
using System.Net;
using System.Web.Mvc;

namespace Comision.Helper.Filters
{
    public class AjaxAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                filterContext.HttpContext.Response.StatusDescription = "کاربر گرامی جهت استفاده از امکانات کامل سایت باید ابتدا وارد شوید.";
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Status = filterContext.HttpContext.Response.StatusCode,
                        isError =true,
                        ErrorMessage = "کاربر گرامی جهت استفاده از امکانات کامل سایت باید ابتدا وارد شوید",
                        LogInUrl = urlHelper.Action("Login", "Account", new { area="",returnUrl = filterContext.HttpContext.Request.Params["returnUrl"] })
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}