using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Comision.Data.Context;
using Comision.Data.Migrations;
using Comision.Helper.Filters;
using Comision.IOC;
using CustomActionFilters;
using Microsoft.AspNet.SignalR;
using StructureMap.Web.Pipeline;
using IDependencyResolver = Microsoft.AspNet.SignalR.IDependencyResolver;

namespace Comision.Ui
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
          //  En2FaConvertor n=new En2FaConvertor();
            SetDbInitializer();
            //Set current Controller factory as StructureMapControllerFactory
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
            //SetDbInitializer();
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            var httpException = exception as HttpException;

            if (httpException != null)
            {
                //////string action;

                //////switch (httpException.GetHttpCode())
                //////{
                //////    case 401:
                //////        // page not Authorize
                //////        action = "NotAuthorize";
                //////        break;
                //////    case 404:
                //////        // page not found
                //////        action = "NotFound";
                //////        break;
                //////    case 403:
                //////        // forbidden
                //////        action = "Forbidden";
                //////        break;
                //////    case 500:
                //////        // server error
                //////        action = "HttpError500";
                //////        break;
                //////    default:
                //////        action = "Unknown";
                //////        break;
                //////}

                // clear error on server
                Server.ClearError();

                Response.Redirect($"~/Home/Error?httpCode={httpException.GetHttpCode()}");
            }
            else
            {
                // this is my modification, which handles any type of an exception.
                Response.Redirect($"~/Home/Error?httpCode=-1");
            }
        }
        public class StructureMapControllerFactory : DefaultControllerFactory
        {
            protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                if (controllerType == null)
                    throw new InvalidOperationException($"Page not found: {requestContext.HttpContext.Request.RawUrl}");
                return SmObjectFactory.Container.GetInstance(controllerType) as Controller;
            }
        }

        private static void SetDbInitializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ComisionDbContext, Configuration>());

            SmObjectFactory.Container.GetInstance<IUnitOfWork>().ForceDatabaseInitialize();
            SmObjectFactory.Container.Configure(x =>
            {
                x.For<IDependencyResolver>().Singleton().Add<StructureMapDependencyResolver>();
            });

            GlobalHost.DependencyResolver =
            SmObjectFactory.Container.GetInstance<IDependencyResolver>();
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahRequestValidationErrorFilter());
            filters.Add(new ElmahHandledErrorLoggerFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
