using System;
using System.Linq;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.ImplementationService;
using Comision.Service.IService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Admin.Controllers
{
    [Authorize(Roles = "AdminUniversity")]
    public partial class PersonController : Controller
    {
        private readonly IPersonManagementService _personManagementService;
        private readonly IRoleManagementService _roleManagementService;
        public PersonController(IPersonManagementService personManagementService, IRoleManagementService roleManagementService)
        {
            _personManagementService = personManagementService;
            _roleManagementService = roleManagementService;
        }
        public virtual ActionResult Index()
        {
            var enumUserType = EnumerationService.GetEnumValues<UserType>();
            var enumUserTypeList = new SelectList(enumUserType, "Value", "Text");
            ViewData["UserType"] = enumUserTypeList;
            return View();
        }
        public virtual ActionResult GetPersons(UserType userType = UserType.Personel, int page = 1, int rows = 10)
        {
            try
            {
                int pagesCount;
                var userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
                var levelId = Convert.ToInt64(User.LevelId());
                var rolesName = string.Empty;
                var postsName = string.Empty;
                var mandatesName = string.Empty;
                var roles = _roleManagementService.GetAllRole(userAuthenticationType, levelId).ToList();
                var posts = _roleManagementService.GetAllPost().ToList();
                var listPerson = _personManagementService.GetUsers(a => a.Id == 1, userAuthenticationType, userType, levelId, true, out pagesCount, page, rows);
                var ja = new JArray();
                foreach (var item in listPerson)
                {
                    var itemObject = new JObject
                    {
                        {"id",item.Id},
                        {"fullName", item.Profile?.FullName},
                        {"email",item.User == null ? "" : item.User.Email},
                        {"avatar",item.Profile == null ? "" : item.Profile.Avatar},
                        {"studentNumber",item.Student?.StudentNumber },
                        {"personelNumber",item.Personel?.EmployeeNumber }
                    };
                    rolesName = item.User == null ? "" : item.User.Roles?.Aggregate(rolesName, (current, itm) => current + (roles.Find(c => c.Id == itm.RoleId).NameFa + " ، "));
                    postsName = item.PostPersons == null ? "" : item.PostPersons.Aggregate(postsName, (current, itm) => current + (posts.Find(c => c.Id == itm.PostId).Name + " ، "));
                    mandatesName = item.User?.UserPosts == null ? "" : item.User.UserPosts.Aggregate(mandatesName, (current, itm) => current + (posts.Find(c => c.Id == itm.PostId).Name + " ، "));
                    itemObject.Add("roleName", rolesName);
                    itemObject.Add("postName", postsName);
                    itemObject.Add("mandateName", mandatesName);
                    ja.Add(itemObject);
                    rolesName = string.Empty;
                    postsName = string.Empty;
                    mandatesName = string.Empty;
                }

                var jo = new JObject { { "total", pagesCount }, { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            catch (Exception exception)
            {
                return Json(new { isError = true, Msg = "خطا در لود اطلاعات اشخاص" });
            }
        }
    }
}