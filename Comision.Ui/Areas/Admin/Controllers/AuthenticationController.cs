using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.ImplementationService;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Admin.Controllers
{
    [Authorize(Roles = "AdminUniversity")]
    public partial class AuthenticationController : Controller
    {
        private readonly IRoleManagementService _roleManagementService;
        private readonly IFileManagementService _fileManagementService;

        public AuthenticationController(IRoleManagementService roleManagerService, IFileManagementService fileManagementService)
        {
            _roleManagementService = roleManagerService;
            _fileManagementService = fileManagementService;
        }
        public virtual ActionResult Index()
        {
            var enumAuthenticationType = EnumerationService.GetEnumValues<AuthenticationType>();
            enumAuthenticationType.RemoveRange(0,3);
            var enumAuthenticationTypeList = new SelectList(enumAuthenticationType, "Value", "Text");
            ViewData["AuthenticationType"] = enumAuthenticationTypeList;
            return View();
        }
        public virtual ActionResult GetAuthentication(int page = 1, int rows = 20)
        {
            try
            {
                int pagesCount;

                var userLoginAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
                var levelId = Convert.ToInt64(User.LevelId());
                var authenticationList = _roleManagementService.GetAuthenticationPaged(a => a.Id == 1, userLoginAuthenticationType, levelId, true, out pagesCount, page, rows).ToList();

                var ja = new JArray();
                foreach (var itemObject in authenticationList.Select(item => new JObject
                {
                    {"Id", item.Id},
                    {"AuthenticationTypeId",(int)item.AuthenticationType},
                    { "AuthenticationType",item.AuthenticationType.GetDescription()},
                    {"IdentityCode",item.IdentityCode},
                    {"Active", item.Users.Count>0}
                }))
                {
                    ja.Add(itemObject);
                }
                var jo = new JObject {{"total", pagesCount}, {"rows", ja}};
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            catch (Exception)
            {
                return Json(new { isError = true, Msg = "خطا در لود اطلاعات کاربران" });
            }
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult AddorUpdate(AuthenticationModel authenticationModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { IsError = true, Message = "ورودی نا معتبر!" });
                }
                //مشخص کردن سطح رکورد احراز هویت بر اساس سطح مربوط به کاربر لاگین شده
                AuthenticationType userAuthenticationType =
                    (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
                if (authenticationModel.Id == null || authenticationModel.Id == 0)
                {
                    if (userAuthenticationType == AuthenticationType.AdminCentral)
                        authenticationModel.CentralOrganizationId = Convert.ToInt64(User.LevelId());
                    else if (userAuthenticationType == AuthenticationType.AdminBranch)
                        authenticationModel.BranchProvinceId = Convert.ToInt64(User.LevelId());
                    else
                        authenticationModel.UniversityId = Convert.ToInt64(User.LevelId());
                }
                var data = _roleManagementService.AddOrUpdateAuthentication(authenticationModel, authenticationModel.Id != null && authenticationModel.Id > 0 ? StateOperation.ویرایش : StateOperation.درج);
                return Json(new { IsError = !data.Item1, Message = data.Item2 });
            }
            catch (DbEntityValidationException)
            {
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    a = eve.Entry.Entity.GetType().Name;
                //    //b=eve.Entry.State;
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        a = ve.PropertyName;
                //        bb=ve.ErrorMessage;
                //    }
                //}
                //throw;
                return Json(new {IsError = true, Message = "خطا در ثبت اطلاعات!" });
            }
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Delete(int id)
        {
            try
            {
                var data = _roleManagementService.DeleteAuthentication(id);
                if (data.Item1)
                    return Json(new { IsError = false, Message = "عملیات حذف انجام شد!" });
                return Json(new { IsError = true, Message = data.Item2 });
            }
            catch (Exception)
            {
                return Json(new { IsError = true, Message = "خطا در حذف" });
            }
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult FileUpload(int authenticationType, HttpPostedFileBase excelFile)
        {
            try
            {
                if (excelFile == null)
                    return Json(new { IsError = true, Message = "خطا در انتخاب فایل" });
                var savetoPath = Server.MapPath("~/App_Data/");
                var data = _fileManagementService.FileUpload(excelFile, savetoPath);
                if (data.Item1)
                {
                    var listAuthentication = _fileManagementService.GetExcelContent(Path.Combine(savetoPath, excelFile.FileName));
                    //مشخص کردن سطح رکورد احراز هویت بر اساس سطح مربوط به کاربر لاگین شده
                    AuthenticationType authen = (AuthenticationType)authenticationType;
                    AuthenticationType userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
                    long levelId = Convert.ToInt64(User.LevelId());

                    var listAuthen = new List<AuthenticationModel>();
                    for (int i = 0; i <= listAuthentication.Rows.Count - 1; i++)
                    {
                        AuthenticationModel authentication = new AuthenticationModel
                        {
                            IdentityCode = listAuthentication.Rows[i][0].ToString(),
                            AuthenticationType = authen
                        };
                        if (userAuthenticationType == AuthenticationType.AdminCentral)
                            authentication.CentralOrganizationId = levelId;
                        else if (userAuthenticationType == AuthenticationType.AdminCentral)
                            authentication.BranchProvinceId = levelId;
                        else
                            authentication.UniversityId = levelId;

                        listAuthen.Add(authentication);
                    }
                    var lstAllAuthentication = _roleManagementService.GetAllAuthentication(userAuthenticationType, levelId)
                        .Select(s => new AuthenticationModel
                        {
                            IdentityCode = s.IdentityCode,
                            AuthenticationType = s.AuthenticationType
                        }).ToList();
                    var listNewAuthentication = listAuthen.Except(lstAllAuthentication, new AuthenticationComparer()).ToList();

                    data = _roleManagementService.AddListAuthentication(listNewAuthentication, StateOperation.درج);

                    return Json(new { IsError = !data.Item1, Message = data.Item2 });
                }
                return Json(new { IsError = true, Message = data.Item2 });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = "خطا در انتقال کاربران" });
            }
        }

        //public virtual ActionResult Users()
        //{
        //    var uId = int.Parse(User.Identity.GetUserId());
        //    var model1 = _commentService.Where(u => u.Id == 1).Include(i => i.UserSender).Include(x => x.UserSender.Profile).ToList();

        //    var model = _userService.Where(x => x.Id != uId && x.Profile != null).Include(x => x.Profile).ToList();
        //    return System.Web.UI.WebControls.View(model);
        //}
    }
}