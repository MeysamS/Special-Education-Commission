using System;
using System.Linq;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Admin.Controllers
{
    [Authorize(Roles = "AdminUniversity")]
    public partial class SpecialEducationController : Controller
    {
        private readonly IBaseInfoComissionCouncilService _baseInfoComissionCouncilService;

        public SpecialEducationController(IBaseInfoComissionCouncilService baseInfoComissionCouncilService)
        {
            _baseInfoComissionCouncilService = baseInfoComissionCouncilService;
        }
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult AddOrUpdate(SpecialEducationModel specialEducationModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { IsError = true, Message = "ورودی نا معتبر!" });
                var data = _baseInfoComissionCouncilService.AddOrUpdateSpecialEducation(specialEducationModel, specialEducationModel.Id != null && specialEducationModel.Id > 0 ? StateOperation.ویرایش : StateOperation.درج);
                return Json(new { IsError = !data.Item1, Message = data.Item2 });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = "خطا در ثبت اطلاعات!" });
            }
        }

        public virtual ActionResult GetSpecialEducation()
        {
            try
            {
                var data = _baseInfoComissionCouncilService.GetAllSpecialEducation().ToList();
                JArray ja = new JArray();

                foreach (var item in data)
                {
                    var itemObject = new JObject
                    {
                        {"Id", item.Id},
                        {"Name",item.Name}
                    };
                    ja.Add(itemObject);
                }
                JObject jo = new JObject();
                jo.Add("total", data.Count);
                jo.Add("rows", ja);
                return Content(JsonConvert.SerializeObject(jo));
            }
            catch (Exception)
            {
                return Json(new { isError = true, Msg = "خطا در لود اطلاعات پست" });
            }
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Delete(long id)
        {
            try
            {
                var data = _baseInfoComissionCouncilService.DeleteSpecialEducation(id);
                return Json(new { IsError = !data.Item1, Message = data.Item2 });
            }
            catch (Exception)
            {
                return Json(new { IsError = true, Message = "خطا در حذف" });
            }
        }
    }
}