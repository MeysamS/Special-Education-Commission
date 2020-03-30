using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Comision.IOC;
using Comision.Service.IService;
using Microsoft.AspNet.Identity;

namespace Comision.Helper.Utility
{
    public static class HtmlExtensions
    {
        public static IHtmlString TextBoxForCostum<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, bool disabled)
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            if (disabled)
            {
                attributes["disabled"] = "disabled";
            }
            return htmlHelper.TextBoxFor(expression, attributes);
        }


        public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string linkText, string Action, string ControllerName, string Area, object routeValues, object htmlAttributes)
        {
            var queryString = string.Empty;
            var htmlAttributesString = string.Empty;
            //My Changes
            var IsRoute = false;
            if (routeValues != null)
            {
                var d = new RouteValueDictionary(routeValues);
                for (var i = 0; i < d.Keys.Count; i++)
                {
                    //My Changes
                    if (!d.Keys.Contains("IsRoute"))
                    {
                        if (i > 0)
                        {
                            queryString += "?";
                        }
                        queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                    }
                    else
                    {
                        //My Changes
                        if (d.Keys.ElementAt(i).Contains("IsRoute")) continue;
                        queryString += d.Values.ElementAt(i);
                        IsRoute = true;
                    }
                }
            }
            if (htmlAttributes != null)
            {
                var d = new RouteValueDictionary(htmlAttributes);
                for (var i = 0; i < d.Keys.Count; i++)
                {
                    htmlAttributesString += " " + d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
            }
            var ancor = new StringBuilder();
            ancor.Append("<a ");
            if (htmlAttributesString != string.Empty)
            {
                ancor.Append(htmlAttributesString);
            }
            ancor.Append(" href='");
            if (Area != string.Empty)
            {
                ancor.Append("/" + Area);
            }

            if (ControllerName != string.Empty)
            {
                ancor.Append("/" + ControllerName);
            }

            if (Action != "Index")
            {
                ancor.Append("/" + Action);
            }
            //My Changes
            if (queryString != string.Empty)
            {
                if (IsRoute == false)
                    ancor.Append("?q=" + Encrypt(queryString));
                else
                    ancor.Append("/" + Encrypt(queryString));
            }
            ancor.Append("'");
            ancor.Append(">");
            ancor.Append(linkText);
            ancor.Append("</a>");
            return new MvcHtmlString(ancor.ToString());
        }

        private static string Encrypt(string plainText)
        {
            var key = "jdsg432387#";
            byte[] encryptKey = { };
            byte[] iv = { 55, 34, 87, 64, 87, 195, 54, 21 };
            encryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var des = new DESCryptoServiceProvider();
            var inputByte = Encoding.UTF8.GetBytes(plainText);
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, des.CreateEncryptor(encryptKey, iv), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }


        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string roleName)
        {
            return SecureActionLink(htmlHelper, linkText, null, null, roleName, new RouteValueDictionary(), new RouteValueDictionary());
        }
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string roleName)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, null, roleName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string roleName, object routeValues)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, null, roleName, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string roleName, object routeValues, object htmlAttributes)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, null, roleName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string roleName, RouteValueDictionary routeValues)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, null, roleName, routeValues, new RouteValueDictionary());
        }

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string roleName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, null, roleName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string roleName)
        {
            return SecureActionLink(htmlHelper, linkText, actionName, controllerName, roleName, new RouteValueDictionary(), new RouteValueDictionary());
        }

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string roleName, object routeValues, object htmlAttributes)
        {
            var varBool = CanAccess(htmlHelper, roleName);
            return varBool
                ? htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes)
                : new MvcHtmlString(string.Empty);
        }
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string roleName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            var varBool = CanAccess(htmlHelper, roleName);
            if (varBool)
            {
                return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            return new MvcHtmlString(string.Empty);
        }
        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string roleName, string innerHtml, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            var varBool = CanAccess(htmlHelper, roleName);
            if (varBool)
            {
                var aBuilder = new TagBuilder("a");
                aBuilder.Attributes.Add("value", linkText);
                foreach (var attr in htmlAttributes)
                {
                    aBuilder.MergeAttribute(attr.Key, attr.Value.ToString());
                }
                aBuilder.InnerHtml += innerHtml;

                return new MvcHtmlString(aBuilder.ToString(TagRenderMode.Normal));
                //return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            return new MvcHtmlString(string.Empty);
        }

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string roleName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            var varBool = CanAccess(htmlHelper, roleName);
            return varBool
                ? htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes)
                : new MvcHtmlString(string.Empty);
        }

        public static bool CanAccess(HtmlHelper htmlHelper, string roleName)
        {
            var accessControlService = SmObjectFactory.Container.GetInstance<IAccessControlService>();
            var userId = Convert.ToInt64(HttpContext.Current.User.Identity.GetUserId());
            var cookiePermission = HttpContext.Current.Request.Cookies.Get("ComisionPermission" + userId);
            if (cookiePermission != null)
            {
                var qbol2 = accessControlService.HasPermission(cookiePermission["permissions"], roleName);
                return qbol2;
            }
            var xmlPermission = string.Join(",", accessControlService.GetUserPermissions(userId).ToArray()); //XmlUtility.ConvertObjectToXml(AccessControl.GetUserPermissions(userId));
            var myCookie = new HttpCookie("ComisionPermission" + userId)
            {
                ["permissions"] = xmlPermission,
                //["UserId"] = userId.ToString(),
                Expires = DateTime.Now.AddSeconds(30)
            };
            HttpContext.Current.Response.Cookies.Add(myCookie);
            var qbol = accessControlService.HasPermission(xmlPermission, roleName);
            return qbol;
        }
    }
}
