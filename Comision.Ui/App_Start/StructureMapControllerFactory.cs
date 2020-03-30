using System;
using System.Web.Mvc;
using System.Web.Routing;
using Comision.IOC;

namespace Comision.Ui
{
    /// <summary>
    /// دیدی نمیشه؟!؟!؟!؟!؟!؟!
    /// </summary>
    public class StructureMapControllerFactory:DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                throw new InvalidOperationException(string.Format("Page not found: {0}", requestContext.HttpContext.Request.RawUrl));
            return SmObjectFactory.Container.GetInstance(controllerType) as Controller;
        }
    }
}