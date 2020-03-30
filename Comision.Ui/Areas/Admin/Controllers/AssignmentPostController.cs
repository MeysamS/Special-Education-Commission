using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.IOC;
using Comision.Model.Enum;
using Comision.Service.ImplementationService;
using Comision.Service.IService;
using Comision.Service.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Admin.Controllers
{
    [Authorize(Roles = "AdminUniversity")]
    public partial class AssignmentPostController : Controller
    {
        private readonly IPersonManagementService _personManagementService;
        private readonly IRoleManagementService _roleManagementService;
        private readonly IStructureManageService _structureManageService;


        public AssignmentPostController(IPersonManagementService personManagementService, IRoleManagementService roleManagementService,
            IStructureManageService structureManageService)
        {
            _personManagementService = personManagementService;
            _roleManagementService = roleManagementService;
            _structureManageService = structureManageService;

        }
        public virtual ActionResult Index(long? personId)
        {
            return View(personId);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult AddOrUpdateAssignmentPost(PostPersonModel postPersonModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { IsError = true, Message = "ورودی نا معتبر!" });

                //var postPersonModel = new PostPersonModel() { PostType = postType, PostId = postId, PersonId = personId, LevelId = 0 };
                var data = _personManagementService.AddOrUpdateAssignmentPost(postPersonModel);
                return Json(new { IsError = !data.Item1, Message = data.Item2 });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = "خطا در ثبت اطلاعات!" });
            }
        }
        public virtual ActionResult GetListPostofPersonel(long personId)
        {
            try
            {
                var data = _personManagementService.GetPostofPersonel(personId).Item3;

                var ja = new JArray();

                foreach (var itemObject in data.Select(item => new JObject
                {
                    {"PersonId", item.PersonId},
                    {"PostId", item.PostId},
                    {"PostName",item.PostName},
                    {"PostTypeId",(int)item.PostType},
                    {"PostType",item.PostType.ToString()},
                    {"LevelId",item.LevelId},
                    {"LevelName",item.LevelName}

                }))
                {
                    ja.Add(itemObject);
                }
                var jo = new JObject { { "total", data.ToList().Count }, { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo));
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = "خطا در لود اطلاعات پرسنل" });
            }
        }
        public virtual ActionResult GetListPersonel()
        {
            try
            {
                var levelProgram = (LevelProgram)Enum.Parse(typeof(LevelProgram), User.LevelProgram(), true);
                var levelId = Convert.ToInt64(User.LevelId());
                var data = _personManagementService.GetPersonelProfiles(levelProgram, levelId).Item3;

                var ja = new JArray();

                foreach (var itemObject in data.Select(item => new JObject
                {
                    {"PersonId", item.PersonId},
                    {"NameFamili",item.ModelProfile.NameFamili},
                    {"NationalCode",item.ModelProfile.NationalCode},
                    {"EmployeeNumber",item.EmployeeNumber},
                    {"DateOfEmployeement",item.DateOfEmployeement}
                }))
                {
                    ja.Add(itemObject);
                }
                var jo = new JObject { { "total", data.ToList().Count }, { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo));
            }
            catch (Exception)
            {
                return Json(new { IsError = true, Message = "خطا در لود اطلاعات پرسنل" });
            }
        }
        public virtual ActionResult GetListPost()
        {
            try
            {
                var data = _roleManagementService.GetAllPost();
                var ja = new JArray();
                foreach (var itemObject in data.Select(item => new JObject
                {
                    { "Id", item.Id},
                    { "Name", item.Name},
                    { "PostType", item.PostType.ToString()},
                    { "PostTypeId", (int) item.PostType}
                }))
                {
                    ja.Add(itemObject);
                }
                var jo = new JObject { { "total", data.ToList().Count }, { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo));
            }
            catch (Exception)
            {
                return Json(new { IsError = true, Message = "خطا در لود اطلاعات پست" });
            }
        }
        public virtual ActionResult GetListStructure(long postId, PostType postType)
        {
            try
            {
                long levelId = Convert.ToInt64(User.LevelId());
                var data = _structureManageService.GetSubStructurebyPost(postType, levelId).Item3;

                List<JObject> jobjectList = data.Select(item => new JObject
                {
                    {"Id", item.Id}, {"Name", item.Name}, {"StructureType", item.StructureType.ToString()}, {"StructureTypeId", (int) item.StructureType}
                }).ToList();
                return Content(JsonConvert.SerializeObject(jobjectList));
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = "خطا در لود اطلاعات زیر ساختار" });
            }
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult DeleteAssignmentPost(long personId, long postId, int postTypeId, long levelId)
        {
            try
            {
                var data = _personManagementService.DeleteAssignmentPost(postId, personId, (PostType)postTypeId, levelId);
                return Json(new { IsError = !data.Item1, Message = data.Item2 });
            }
            catch (Exception)
            {
                return Json(new { IsError = true, Message = "خطا در حذف" });
            }
        }
    }
}