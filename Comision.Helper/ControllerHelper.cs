using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Service.Model;

namespace Comision.Helper
{
    /// <summary>
    /// نمایش لیست از کنتر ها و اکشن ها
    /// A Utility class for MVC controllers.
    /// </summary>
    public class ControllerHelper
    {
        /// <summary>
        /// Gets the controllers name an description with their actions.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Lazy&lt;IEnumerable&lt;ControllerDescription&gt;&gt;.</returns>
        public IEnumerable<ControllerModel> GetControllersNameAnDescription(string filter = null)
        {
            var assembly = Assembly.GetCallingAssembly();
            var controllers = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Controller)) && !type.Name.ToLower().Contains("t4mvc") &&
                (DescriptionAttribute)Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute)) != null)
                .ToList();
            if (!string.IsNullOrWhiteSpace(filter))
                controllers = controllers.Where(t => !t.Name.Contains(filter)).ToList();
            var controllerList = (
                    from controller in controllers
                    let attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(controller, typeof(DescriptionAttribute))
                    //let ctrlDescription = attribute == null ? "" : attribute.Description
                    select new ControllerModel
                    {
                        Name = controller.Name.Replace("Controller", ""),
                        Description = attribute == null ? "" : attribute.Description,
                        Actions = GetActionList(controller)
                    }
                ).ToList();
            return controllerList;
        }

        /// <summary>
        /// Gets the action list.
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns>IEnumerable&lt;ActionDescription&gt;.</returns>
        private static IEnumerable<ActionModel> GetActionList(Type controllerType)
        {
            var actions = new ReflectedControllerDescriptor(controllerType).GetCanonicalActions()
                .Where(a =>
             {
                 var customAuthorizeAttribute = (CustomAuthorizeAttribute)a.GetCustomAttributes(typeof(CustomAuthorizeAttribute), false).FirstOrDefault();
                 return customAuthorizeAttribute != null && customAuthorizeAttribute.AllowInAccess;
             }).ToList();
            ///   .Where(a => ((CustomAuthorizeAttribute)a.GetCustomAttributes(typeof(CustomAuthorizeAttribute), false).FirstOrDefault()).AllowInAccess).ToList();


            var actionList = (from actionDescriptor in actions
                              let attribute = actionDescriptor.GetCustomAttributes(typeof(CustomAuthorizeAttribute), false).FirstOrDefault() as CustomAuthorizeAttribute
                              select new ActionModel
                              {
                                  Name = attribute.RoleNameDefault.Split('@')[0],
                                  Description = attribute.RoleNameParametrFa ?? attribute.RoleNameDefault.Split('@')[1]

                              }).ToList();
            actionList = actionList.GroupBy(a => a.Name).Select(g => g.First()).ToList();
            return actionList;
        }
    }
}