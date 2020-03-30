using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Ui.Areas.Admin.Models;
using Comision.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Admin.Controllers
{
    public partial class TextDefaultController : Controller
    {
        private readonly ITextDefaultService _textDefaultService;
        public TextDefaultController(ITextDefaultService textDefaultService)
        {
            _textDefaultService = textDefaultService;
        }

        [Authorize(Roles = "AdminUniversity")]
        public virtual ActionResult Index(TextDefaultType type)
        {
            ViewBag.TextDefaultTypeDescription = type.GetDescription();
            ViewBag.TextDefaultType = type;
            return View();
        }

        public virtual ActionResult GetTextDefault(TextDefaultType type)
        {
            try
            {
                var data = _textDefaultService.GetByType(Convert.ToInt64(User.LevelId()), type);
                JArray ja = new JArray();

                foreach (var item in data)
                {
                    var itemObject = new JObject
                    {
                        {"Id", item.Id},
                        {"Text",item.Text},
                        {"TextDefaultType",item.TextDefaultType.GetDescription()},
                        {"TextDefaultTypeId",(int)item.TextDefaultType}
                    };
                    ja.Add(itemObject);
                }
                JObject jo = new JObject();
                jo.Add("total", data.Count());
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
        public virtual ActionResult AddOrUpdate(TextDefaultViewModel model)
        {
            try
            {
                Tuple<bool, string> state;
                ModelState.Remove("Id");
                if (!ModelState.IsValid)
                    return Json(new { isError = true, Msg = "ورودی نا معتبر!" });

                var text = new TextDefault
                {
                    Id = model.Id ?? 0,
                    UnivercityId = Convert.ToInt64(User.LevelId()),
                    Text = model.Text,
                    TextDefaultType = model.TextDefaultType
                };
                if (model.Id == null || model.Id == 0)
                    state = _textDefaultService.AddOrUpdate(text, StateOperation.درج);
                else
                    state = _textDefaultService.AddOrUpdate(text, StateOperation.ویرایش);

                return Json(new { isError = false, Msg = state.Item2 });
            }
            catch (DbEntityValidationException ex)
            {
                return Json(new { isError = true, Msg = "خطا در ثبت اطلاعات!" });
            }
        }

        [Authorize(Roles = "AdminUniversity")]
        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Delete(long id)
        {
            var sate = _textDefaultService.Delete(id);
            return Json(new { isError = false, Msg = sate.Item2 });
        }

        [HttpGet]
        [AjaxOnly]
        public virtual ActionResult SelectedTextDefault(TextDefaultType type)
        {
            ViewBag.TextDefaultType = type;
            return PartialView("_SelectedTextDefault");
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
    }
}