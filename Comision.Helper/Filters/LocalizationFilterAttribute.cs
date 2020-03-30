using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Comision.Helper.Filters
{
   public class LocalizationFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var locales = new Dictionary<string,string>();
            locales.Add("mx", "es-MX");
            locales.Add("sp", "es-ES");
            locales.Add("vi", "vi-VN");
            locales.Add("fi", "fi-FI");
            var subdomain = GetSubDomain();
            if (subdomain != string.Empty && locales.ContainsKey(subdomain))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(locales[subdomain]);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(locales[subdomain]);

                HttpContext.Current.Response.Write(String.Format("Culture: {0}", Thread.CurrentThread.CurrentCulture.Name));
            }
            else
            {
                HttpContext.Current.Response.Write("Culture: Default ");
            }
            base.OnActionExecuting(filterContext);
        }
        private string GetSubDomain()
        {
            var url = HttpContext.Current.Request.Headers["HOST"];
            var index = url.IndexOf(".");

            if (index < 0)
            {
                return string.Empty;
            }

            var subdomain = url.Split('.')[0];
            if (subdomain == "www" || subdomain == "localhost")
            {
                return string.Empty;
            }

            return subdomain;
        }
    }
}
