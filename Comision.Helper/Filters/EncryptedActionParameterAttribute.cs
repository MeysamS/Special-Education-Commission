using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Comision.Helper.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int Id;
            var decryptedParameters = new Dictionary<string, object>();
            if (HttpContext.Current.Request.QueryString.Get("q") != null)
            {
                var encryptedQueryString = HttpContext.Current.Request.QueryString.Get("q");
                //string decrptedString = Decrypt(encryptedQueryString.ToString());
                var decrptedString = Decrypt(encryptedQueryString.ToString());
                var paramsArrs = decrptedString.Split('?');

                foreach (var paramArr in paramsArrs.Select(t => t.Split('=')))
                {
                    decryptedParameters.Add(paramArr[0], Convert.ToInt32(paramArr[1]));
                }
            }
            else if (!HttpContext.Current.Request.Url.ToString().Contains("?"))
            {
                //if (int.TryParse(Decrypt(HttpContext.Current.Request.Url.ToString().Split('/').Last()), out Id))
                if (int.TryParse(Decrypt(HttpContext.Current.Request.Url.ToString().Split('/').Last()), out Id))
                {
                    var id = Id.ToString();
                    decryptedParameters.Add("id", id);
                }
            }
            else if (HttpContext.Current.Request.Url.ToString().Contains("?"))
            {
                //if (int.TryParse(Decrypt(HttpContext.Current.Request.Url.ToString().Split('/').Last().Split('?')[0]), out Id))
                if (int.TryParse(Decrypt(HttpContext.Current.Request.Url.ToString().Split('/').Last().Split('?')[0]), out Id))
                {
                    var id = Id.ToString();
                    if (id.Contains('?'))
                    {
                        id = id.Split('?')[0];
                    }
                    decryptedParameters.Add("id", id);
                }

                var paramsArrs = HttpContext.Current.Request.Url.ToString().Split('/').Last().Split('?');
                for (var i = 1; i < paramsArrs.Length; i++)
                {
                    var paramArr = paramsArrs[i].Split('=');
                    decryptedParameters.Add(paramArr[0], Convert.ToInt32(paramArr[1]));
                }
            }

            for (var i = 0; i < decryptedParameters.Count; i++)
            {
                filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = decryptedParameters.Values.ElementAt(i);
            }
            base.OnActionExecuting(filterContext);
        }

        private string Decrypt(string encryptedText)
        {
            const string key = "jdsg432387#";
            byte[] decryptKey = { };
            byte[] iv = { 55, 34, 87, 64, 87, 195, 54, 21 };
            var inputByte = new byte[encryptedText.Length];

            decryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var des = new DESCryptoServiceProvider();
            inputByte = Convert.FromBase64String(encryptedText);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(decryptKey, iv), CryptoStreamMode.Write);
            cs.Write(inputByte, 0, inputByte.Length);
            cs.FlushFinalBlock();
            var encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{

        //    for (int i = 0; i < filterContext.ActionParameters.Count; i++)
        //    {
        //        if (!filterContext.ActionParameters.Keys.ElementAt(i).Contains("noenc"))
        //        {
        //            filterContext.ActionParameters[filterContext.ActionParameters.Keys.ElementAt(i)] =
        //            Decrypt(filterContext.ActionParameters.ElementAt(i).Value.ToString());
        //        }

        //    }
        //    base.OnActionExecuting(filterContext);
        //}

        //private string Decrypt(string encryptedText)
        //{
        //    string key = "jdsg3267247#";
        //    byte[] DecryptKey = { };
        //    byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
        //    byte[] inputByte = new byte[encryptedText.Length];
        //    DecryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    inputByte = Convert.FromBase64String(encryptedText);
        //    MemoryStream ms = new MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(DecryptKey, IV), CryptoStreamMode.Write);
        //    cs.Write(inputByte, 0, inputByte.Length);
        //    cs.FlushFinalBlock();
        //    Encoding encoding = Encoding.UTF8;
        //    return encoding.GetString(ms.ToArray());
        //}
    }
}
