using System;
using System.Web;
using System.Web.Mvc;
using Comision.IOC;
using Comision.Service.IService;
using Microsoft.AspNet.Identity;

namespace Comision.Helper.Filters
{
    [AttributeUsage(AttributeTargets.All | AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string RoleNameDefault;// پیش فرض
        public string RoleNameParametrFa;//سفارشی فارسی
        public bool AllowInAccess;// دسترسی شامل شود

        private IAccessControlService _accessControlService;

        //public CustomAuthorizeAttribute(IAccessControlService accessControlService)
        //{
        //    _accessControlService = accessControlService;
        //}
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //if (string.IsNullOrWhiteSpace(RoleNameEn))
            //    RoleNameEn = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //if (string.IsNullOrWhiteSpace(RoleNameFa))
            //    RoleNameFa = filterContext.ActionDescriptor.ActionName;
            _accessControlService = SmObjectFactory.Container.GetInstance<IAccessControlService>();
            base.OnAuthorization(filterContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
                return false;
            var userId = Convert.ToInt64(user.Identity.GetUserId());
            var cookiePermission = HttpContext.Current.Request.Cookies.Get("ComisionPermission" + userId);
            if (cookiePermission != null)
            {
                var qbol2 = _accessControlService.HasPermission(cookiePermission["permissions"], RoleNameDefault);
                return qbol2;
            }
            var xmlPermission = string.Join(",", _accessControlService.GetUserPermissions(userId).ToArray()); //XmlUtility.ConvertObjectToXml(_accessControl.GetUserPermissions(userId));
            var myCookie = new HttpCookie("ComisionPermission" + userId)
            {
                ["permissions"] = xmlPermission,
                //["UserId"] = userId.ToString(),
                Expires = DateTime.Now.AddSeconds(30)
            };
            HttpContext.Current.Response.Cookies.Add(myCookie);
            var qbol = _accessControlService.HasPermission(xmlPermission, RoleNameDefault);
            return qbol;
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
