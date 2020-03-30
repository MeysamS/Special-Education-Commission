using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Comision.Helper;
using Comision.Helper.Filters;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Admin.Controllers
{
   
    [Authorize(Roles = "AdminUniversity")]
    public partial class RoleController : Controller
    {
        private static Lazy<IEnumerable<ControllerModel>> _controllerList = new Lazy<IEnumerable<ControllerModel>>();
        private readonly IRoleManagementService _roleManagementService;

        public RoleController(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        // GET: Admin/Role
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult CreateOrUpdate(RoleModel roleModel)//List<ControllerDescription> controllers,
        {

            if (!ModelState.IsValid)
                return Json(new { IsError = true, Message = "ورودی نامعتبر!" });

            var userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
            if (roleModel.Id == null || roleModel.Id == 0)
            {
                switch (userAuthenticationType)
                {
                    case AuthenticationType.AdminCentral:
                        roleModel.CentralOrganizationId = Convert.ToInt64(User.LevelId());
                        break;
                    case AuthenticationType.AdminBranch:
                        roleModel.BranchProvinceId = Convert.ToInt64(User.LevelId());
                        break;
                    default:
                        roleModel.UniversityId = Convert.ToInt64(User.LevelId());
                        break;
                }
                roleModel.RoleType = RoleType.None;
            }
            var result = _roleManagementService.AddOrUpdateRole(roleModel, roleModel.Id != null && roleModel.Id > 0 ? StateOperation.ویرایش : StateOperation.درج);
            return Json(new { IsError = !result.Item1, Message = result.Item2 });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Delete(long id)
        {
            var result = _roleManagementService.DeleteRole(id);
            return Json(new { IsError = !result.Item1, Message = result.Item2 });
        }

        public virtual ActionResult GetRoles(int page = 1, int rows = 10)
        {
            try
            {
                int pagesCount;
                var userLoginAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
                var levelId = Convert.ToInt64(User.LevelId());
                var roles = _roleManagementService.GetRolePaged(r => r.Id == 1, userLoginAuthenticationType, levelId, true, out pagesCount, page, rows).ToList();
                var ja = new JArray();
                foreach (var itemObject in roles.Select(item => new JObject
                {
                    {"Id",item.Id},
                    {"Name",item.Name},
                    {"NameFa",item.NameFa}
                }))
                {
                    ja.Add(itemObject);
                }
                var jo = new JObject { { "total", pagesCount }, { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo));
            }
            catch (Exception)
            {
                return Json(new { isError = true, Msg = "خطا در بارگزاری نقش ها" });
            }
        }
        public virtual ActionResult GetActions(long roleId)
        {
            try
            {
                var roleAccess = _roleManagementService.GetAccessRoleXml(roleId).Item2;
                var controllers = GetControllers();
                var controllerDescriptions = controllers as ControllerModel[] ?? controllers.ToArray();
                var jobjects = new List<JObject>();
                var root = new JObject
                {
                    { "id", 0 },
                    { "text", "آیتم های دسترسی" },
                    { "hasChild", controllerDescriptions.Any() },
                    { "checked", false }
                };
                var jarray = new JArray();
                if (controllerDescriptions.Any())
                    foreach (var controller in controllerDescriptions)
                    {
                        var jarray2 = new JArray();
                        var jcontroller = new JObject
                        {
                            { "id", controller.Name },
                            { "text", controller.Description },
                            { "hasChild", controller.Actions.Any() },
                            { "checked", false }//(roleAccess!=null && roleAccess.Any(a=>a.Name == controller.Name))
                        };
                        if (controller.Actions.Any())
                        {
                            foreach (var action in controller.Actions.Select(jaction => new JObject
                            {
                                { "id", jaction.Name },
                                { "text", jaction.Description },
                                { "hasChild", false },
                                { "checked", (roleAccess!=null && roleAccess.Any(a=>a.Name ==controller.Name && a.Actions!=null && a.Actions.Any(aa=>aa.Name == jaction.Name))) }
                            }))
                            {
                                jarray2.Add(action);
                            }
                        }
                        jcontroller.Add("children", jarray2);
                        jarray.Add(jcontroller);
                    }
                root.Add("children", jarray);
                jobjects.Add(root);
                return Content(JsonConvert.SerializeObject(jobjects));
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, Msg = "خطا در بارگزاری سطوح دسترسی" });
            }
        }
        private static IEnumerable<ControllerModel> GetControllers()
        {
            if (_controllerList.IsValueCreated)
                return _controllerList.Value;

            _controllerList = new Lazy<IEnumerable<ControllerModel>>(() => new ControllerHelper().GetControllersNameAnDescription());
            return _controllerList.Value;
        }
    }
}