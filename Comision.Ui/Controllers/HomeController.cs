using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Enum;
using Comision.Service.IService;
using Comision.Ui.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Controllers
{
    [Expire]
    public partial class HomeController : Controller
    {
        private readonly IAuthenticationManagementService _authenticationManagementService;
        private readonly ITextDefaultService _textDefaultService;
        public HomeController(IAuthenticationManagementService authenticationManagementService,
            ITextDefaultService textDefaultService)
        {
            _authenticationManagementService = authenticationManagementService;
            _textDefaultService = textDefaultService;
        }

        public virtual ActionResult Index()
        {
            var loadProgram = _authenticationManagementService.LoadProgram();
            if (loadProgram.Item1)
                return RedirectToAction(MVC.Account.Login(""));
            switch (loadProgram.Item2)
            {
                case RoleType.AdminCentral:
                    return RedirectToAction(MVC.Account.RegisterProgramCentralOrganization());
                case RoleType.AdminBranch:
                    return RedirectToAction(MVC.Account.RegisterProgramBranchProvince());
            }
            return RedirectToAction(MVC.Account.RegisterProgramUniversity());
        }

        public virtual ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public virtual PartialViewResult GetPartialTextDefault(string divName, TextDefaultType textDefaultType, string idboxName, string textboxName)
        {
            ViewBag.divName = divName;
            ViewBag.idboxName = idboxName;
            ViewBag.textboxName = textboxName;
            ViewBag.TextDefaultType = textDefaultType;
            return PartialView(MVC.Home.Views._GetPartialTextDefault);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult GetSelectTextDefault(TextDefaultType type)
        {
            try
            {
                long uniId = Convert.ToInt64(User.LevelId());
                var textDefault = _textDefaultService.GetByType(uniId, type).ToList();
                var ja = textDefault.Select(item => new JObject
                {
                    {"value", item.Id},
                    { "text", item.Text}
                }).ToList();
                return Content(JsonConvert.SerializeObject(ja), "application/json");
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, Message = "خطا در بارگزاری!" });
            }
        }
        public virtual Captcha.CaptchImageAction Image()
        {
            var randomText = SelectRandomWord(4).ToLower();
            Session["captcha"] = randomText;
            if (HttpContext.Session != null) HttpContext.Session["RandomText"] = randomText;
            return new Captcha.CaptchImageAction
            {
                BackgroundColor = Color.BurlyWood,
                RandomTextColor = Color.DarkBlue,
                RandomText = randomText
            };
        }
        private string SelectRandomWord(int numberOfChars)
        {
            if (numberOfChars > 36)
            {
                throw new InvalidOperationException("Random Word Characters cannot be greater than 36");
            }
            var columns = new char[36];
            for (int charPos = 65; charPos < 65 + 26; charPos++)
                columns[charPos - 65] = (char)charPos;
            for (int intPos = 48; intPos <= 57; intPos++)
                columns[26 + (intPos - 48)] = (char)intPos;
            var randomBuilder = new StringBuilder();
            var randomSeed = new Random();
            for (var incr = 0; incr < numberOfChars; incr++)
            {
                randomBuilder.Append(columns[randomSeed.Next(36)].ToString());
            }
            return randomBuilder.ToString();
        }
        public virtual ActionResult Error(int? httpCode)
        {
            switch (httpCode)
            {
                case 401:
                    // page not Authorize
                    return View(MVC.Errors.Views.NotAuthorize);
                case 404:
                    // page not found
                    return View(MVC.Errors.Views.NotFound);
                case 403:
                    // forbidden
                    return View(MVC.Errors.Views.Forbidden);
                case 500:
                    // server error
                    return View(MVC.Errors.Views.HttpError500);
                default:
                    return View(MVC.Errors.Views.Unknown);
            }
        }
    }
}