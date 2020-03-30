using System.Web.Mvc;

namespace Comision.Helper.Filters
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}