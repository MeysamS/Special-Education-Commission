using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.IService;
using Comision.Service.Model;

namespace Comision.Service.ImplementationService
{
    public class AccessControlService : IAccessControlService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        public AccessControlService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public List<ControllerModel> ConvertXmlToListControllers(string xml)
        {
            try
            {
                XDocument xdoc1;
                using (var s = new StringReader(xml))
                {
                    xdoc1 = XDocument.Load(s);
                }

                // Student objStudent = new Student();
                var xElement = xdoc1.Element("AccessRoles");
                if (xElement == null) return null;
                var controllers
                    = (from item in xElement.Elements("Controller")
                       let element = item.Element("Name")
                       where element != null
                       let xElement1 = item.Element("Description")
                       where xElement1 != null
                       let element1 = item.Element("Actions")
                       where element1 != null
                       select new ControllerModel
                       {
                           Name = element.Value,
                           Description = xElement1.Value,
                           Actions = (from action in element1.Elements("Action")
                                      let element2 = action.Element("Name")
                                      where element2 != null
                                      let xElement2 = action.Element("Description")
                                      where xElement2 != null
                                      select new ActionModel
                                      {
                                          Name = element2.Value,
                                          Description = xElement2.Value
                                      }).ToList()
                       }).ToList();

                return controllers;
            }
            catch (Exception )
            {
                //throw new ArgumentException("",ex);
                return null;
            }
        }


        public string ConvertListControllerToXml(List<ControllerModel> controllers)
        {
            if (controllers == null) return null;
            var doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-16", null);
            doc.AppendChild(docNode);
            XmlNode accessRoles = doc.CreateElement("AccessRoles");
            doc.AppendChild(accessRoles);
            foreach (var control in controllers)
            {
                // اضافه کرد یک نود و پراپرتی ها
                XmlNode controllerNode = doc.CreateElement("Controller");// نود کنترل اضافه شد

                var nameControllerAttribute = doc.CreateAttribute("Name");// پراپرتی نام اضافه شد
                nameControllerAttribute.Value = control.Name;// مقدار پراپرتی نام اضافه شد
                controllerNode.Attributes?.Append(nameControllerAttribute);// اگر نال نبود

                XmlAttribute decControllerAttribute = doc.CreateAttribute("Description");// پراپرتی توضیحات فارسی اضافه شد
                decControllerAttribute.Value = control.Description;// مقدار پراپرتی توضیحات اضافه شد
                controllerNode.Attributes?.Append(decControllerAttribute);// اگر نال نبود

                XmlNode actions = doc.CreateElement("Actions");
                controllerNode.AppendChild(actions);// اضافه شدن اکشن های کنترل
                foreach (var actionItem in control.Actions)
                {
                    XmlNode actionNode = doc.CreateElement("Action");// نود اکشن اضافه شد

                    XmlAttribute nameActionAttribute = doc.CreateAttribute("Name");// پراپرتی نام اضافه شد
                    nameActionAttribute.Value = actionItem.Name;// مقدار پراپرتی نام اضافه شد
                    actionNode.Attributes?.Append(nameActionAttribute);// اگر نال نبود

                    XmlAttribute decActionAttribute = doc.CreateAttribute("Description");// پراپرتی توضیحات اضافه شد
                    decActionAttribute.Value = actionItem.Description;// مقدار پراپرتی توضیحات اضافه شد
                    actionNode.Attributes?.Append(decActionAttribute);// اگر نال نبود

                    actions.AppendChild(actionNode);// نود اکشن اضافه شد به ریشه اکشنن های کنترل
                }
                controllerNode.AppendChild(actions);// اکشنز های کنرل اضافه شد

                accessRoles.AppendChild(controllerNode);// نود کنترل اضافه شد به ایکس ام ال
            }
            return doc.ToString();
        }
        public List<string> GetUserPermissions(long userId)
        {
            var userRoles = GetUserRoles(userId);
            var userPermissions = new List<PermissionModel>();
            List<ControllerModel> controllerDescription;
            foreach (var userRole in userRoles)
            {
                controllerDescription = ConvertXmlToListControllers(userRole.XmlRoleAccess);
                foreach (var action in controllerDescription.SelectMany(controller => controller.Actions.Where(action => userPermissions.Any(a => a.RoleName == action.Name) == false)))
                {
                    userPermissions.Add(new PermissionModel { RoleName = action.Name });
                }
            }
            return userPermissions.Select(s => s.RoleName).ToList();
        }
        public List<Role> GetUserRoles(long userId)
        {
            return _userRoleRepository.Where(x => x.UserId == userId && x.Role.RoleType == RoleType.None).Select(s => s.Role).ToList();
            //return _dbContext.Users.Where(x => x.Id == userId).SelectMany(s => s.Roles.Select(ss => ss.Role)).ToList();
        }

        //public bool HasPermission(long userId, string roleNameEn)
        //{
        //    try
        //    {
        //        var userPermissions = GetUserPermissions(userId);
        //        return (userPermissions.Any(x => x.RoleName == roleNameEn));
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public bool HasPermission(string xmlPermission, string roleNameEn)
        {
            try
            {
                var userPermissions = xmlPermission.Split(','); //XmlUtility.ConvertXmlToObject<List<PermissionModel>>(xmlPermission);
                var tempRoleNames = roleNameEn.Split(',');
                var roleNames = tempRoleNames.Select(s => s.Split('@')[0]).ToList();
                return roleNames.Any(item => userPermissions.Any(x => x == item.ToString()));
            }
            catch
            {
                return false;
            }
        }
    }
}
