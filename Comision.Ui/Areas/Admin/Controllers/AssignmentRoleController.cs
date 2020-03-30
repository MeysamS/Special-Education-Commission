using System;
using System.Linq;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Admin.Controllers
{
    [Authorize(Roles = "AdminUniversity")]
    public partial class AssignmentRoleController : Controller
    {
        private readonly IPersonManagementService _personManagementService;
        private readonly IRoleManagementService _roleManagementService;
        private readonly IStructureManageService _structureManageService;
        public AssignmentRoleController(IPersonManagementService personManagementService, IRoleManagementService roleManagementService,
            IStructureManageService structureManageService)
        {
            _personManagementService = personManagementService;
            _roleManagementService = roleManagementService;
            _structureManageService = structureManageService;
        }
        public virtual ActionResult Index(long? userId)
        {
            return View(userId);
        }
        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult AddOrUpdateAssignmentRole(UserRoleModel userRoleModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { isError = true, Msg = "ورودی نا معتبر!" });

                var data = _personManagementService.AddOrUpdateAssignmentRole(userRoleModel);
                return Json(new { isError = !data.Item1, Msg = data.Item2 });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, Msg = "خطا در ثبت اطلاعات!" });
            }
        }
        public virtual ActionResult GetUserRoles(long userId)
        {
            try
            {
                var data = _personManagementService.GetUserRoles(userId).Item3;

                JArray ja = new JArray();

                foreach (var item in data)
                {
                    var itemObject = new JObject
                    {
                        {"UserId", item.UserId},
                        {"RoleId", item.RoleId},
                        {"RoleName",item.RoleName},
                        {"RoleNameFa",item.RoleNameFa}
                    };
                    ja.Add(itemObject);
                }
                JObject jo = new JObject { { "total", data.ToList().Count }, { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo));
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, Msg = "خطا در لود اطلاعات پرسنل" });
            }
        }
        public virtual ActionResult GetListUser()
        {
            try
            {
                var levelProgram = (LevelProgram)Enum.Parse(typeof(LevelProgram), User.LevelProgram(), true);
                var levelId = Convert.ToInt64(User.LevelId());
                var data = _personManagementService.GetUsers(levelProgram, levelId).Item3;

                var ja = new JArray();


                foreach (var item in data)
                {
                    var itemObject = new JObject
                    {
                        {"UserId", item.UserId},
                        {"NameFamili", item.FullName},
                        {"NationalCode", item.NationalCode},
                        {"UserName", item.UserName}
                    };
                    ja.Add(itemObject);
                }

                var jo = new JObject { { "total", data.ToList().Count }, { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo));
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, Msg = "خطا در لود اطلاعات پرسنل" });
            }
        }
        public virtual ActionResult GetListRole()
        {
            try
            {
                AuthenticationType userLoginAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
                long levelId = Convert.ToInt64(User.LevelId());

                var data = _roleManagementService.GetAllRole(userLoginAuthenticationType, levelId);
                var ja = new JArray();
                foreach (var item in data)
                {
                    var itemObject = new JObject
                    {
                        {"Id", item.Id},
                        {"Name", item.Name},
                        {"NameFa", item.NameFa},
                        {"RoleType", item.RoleType.ToString()},
                        {"RoleTypeId", (int) item.RoleType}
                    };
                    ja.Add(itemObject);
                }

                var jo = new JObject { { "total", data.ToList().Count }, { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo));
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, Msg = "خطا در لود اطلاعات نقش" });
            }
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult DeleteAssignmentRole(long userId, long roleId)
        {
            try
            {
                var data = _personManagementService.DeleteAssignmentRole(userId, roleId);
                return Json(new { isError = !data.Item1, Msg = data.Item2 });
            }
            catch (Exception)
            {
                return Json(new { isError = true, Msg = "خطا در حذف" });
            }
        }
    }
}