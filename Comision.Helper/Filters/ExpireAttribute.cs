using System;
using System.Web.Mvc;

namespace Comision.Helper.Filters
{
    public class ExpireAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (DateTime.Now.Year > 2015)
            //{
            //    filterContext.Result = new ViewResult
            //    {
            //        ViewName = "Expire"
            //    };
            //}
            //base.OnActionExecuting(filterContext);
        }
    }
}