using System.Web.Mvc;
using System.Web.Routing;

namespace Comision.Ui
{
    /// <summary>
    /// dfdfdf
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{*browserlink}", new { browserlink = @".*/arterySignalR/ping" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Slug", // Route name
                "pages/{slug}", // URL with parameters
                new { controller = "Home", action = "Index" } // Parameter defaults
            );

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { $"{typeof(RouteConfig).Namespace}.Controllers" }
            );

            // Add this code to handle non-existing urls
            routes.MapRoute("404-PageNotFound",
                // This will handle any non-existing urls
                "{*url}",
                // "Shared" is the name of your error controller, and "Error" is the action/page
                // that handles all your custom errors
                new { controller = "Home", action = "Error", httpCode = UrlParameter.Optional }
            );
        }
    }
}
