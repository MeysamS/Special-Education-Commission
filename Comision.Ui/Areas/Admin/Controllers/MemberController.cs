using System;
using System.Linq;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Admin.Controllers
{
    [Authorize(Roles = "AdminUniversity")]
    public partial class MemberController : Controller
    {
        private readonly IMemberManagementService _memberManagementService;
        private readonly IRoleManagementService _roleManagementService;
        private readonly IPersonManagementService _personManagementService;
        public MemberController(IMemberManagementService memberManagementService,
            IRoleManagementService roleManagementService, IPersonManagementService personManagementService)
        {
            _memberManagementService = memberManagementService;
            _roleManagementService = roleManagementService;
            _personManagementService = personManagementService;
        }

        public virtual ActionResult Index(RequestType requestType)
        {
            ViewBag.RequestType = requestType;
            return View();
        }

        public virtual ActionResult GetMembers(RequestType requestType)
        {
            var members = _memberManagementService.GetMasters(1, requestType);
            var ja = new JArray();

            foreach (var item in members)
            {
                var itemObject = new JObject
                {
                    {"Id",item.Id},
                    {"Date",item.Date.ToPeString()},
                    {"Name",item.Name}
                };
                ja.Add(itemObject);
            }
            var jo = new JObject { { "rows", ja } };
            //jo.Add("total", pagesCount);
            return Content(JsonConvert.SerializeObject(jo));
        }

        public virtual ActionResult GetDetails(long masterId)
        {
            var details = _memberManagementService.GetDetail(masterId).ToList();
            JArray ja = new JArray();

            foreach (var itemObject in details.Select(item => new JObject
            {
                {"Id",item.Id},
                {"MemberMasterId",item.MemberMasterId},
                //{"PersonId",item.PersonId},
                {"PersonName",item.PersonName},//item.Person.Profile.FullName
                {"PostName",item.PostName},
                {"RowNumber",item.RowNumber}
            }))
            {
                ja.Add(itemObject);
            }
            var jo = new JObject { { "rows", ja } };
            //jo.Add("total", pagesCount);
            return Content(JsonConvert.SerializeObject(jo));
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
                    {"NameFamily",item.ModelProfile.NameFamili},
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
            catch (Exception exception)
            {
                return Json(new { isError = true, Msg = "خطا در لود اطلاعات پرسنل" });
            }
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult CreateMember(MemberMasterModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { IsError = true, Message = "ورودی نامعتبر" });

            var master = new MemberMaster
            {
                Date = model.Date.ToMiladiDate(),
                Name = model.Name,
                RequestType = model.RequestType,
                UniversityId = Convert.ToInt64(User.LevelId())
            };
            var result = _memberManagementService.AddOrUpdateMaster(master, StateOperation.درج);
            return Json(new { IsError = !result.Item1, Message = result.Item2, master.Id });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult CreateDetail(MemberDetailModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { IsError = true, Message = "ورودی نامعتبر" });

            var detail = new MemberDetails { PersonName = model.PersonName, MemberMasterId = model.MemberMasterId, RowNumber = model.RowNumber, PostName = model.PostName };
            var result = _memberManagementService.AddOrUpdateDetail(detail, StateOperation.درج);
            return Json(new { IsError = !result.Item1, Message = result.Item2, detail.Id });
        }

        [HttpPost]
        public virtual ActionResult DeleteMember(long id)
        {
            var result = _memberManagementService.DeleteMaster(id);
            return Json(new { IsError = !result.Item1, Message = result.Item2 });
        }

        [HttpPost]
        public virtual ActionResult DeleteDetail(long id)
        {
            var result = _memberManagementService.DeleteDetail(id);
            return Json( new { IsError = !result.Item1, Message = result.Item2 });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult EditMaster(MemberMasterModel model)
        {
            var master = new MemberMaster { Id = (long)model.Id, Date = model.Date.ToMiladiDate(), Name = model.Name };
            var result = _memberManagementService.AddOrUpdateMaster(master, StateOperation.ویرایش);
            return Json(new { IsError = !result.Item1, Message = result.Item2 });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult EditDetail(MemberDetailModel model)
        {
            var detail = new MemberDetails { Id = (long)model.Id, PersonName = model.PersonName, PostName = model.PostName, RowNumber = model.RowNumber };
            var result = _memberManagementService.AddOrUpdateDetail(detail, StateOperation.ویرایش);
            return Json(new { IsError = !result.Item1, Message = result.Item2 });
        }

        public virtual ActionResult GetPosts()
        {
            var items = _roleManagementService.GetAllPost();
            var ja = new JArray();
            foreach (var itemObject in items.Select(item => new JObject
            {
                {"PostId", item.Name},
                {"PostName",item.Name}
            }))
            {
                ja.Add(itemObject);
            }
            return Content(JsonConvert.SerializeObject(ja), "application/json");
        }
    }
}