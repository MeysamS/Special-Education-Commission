using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Admin.Controllers
{
    /// <summary>
    /// اعضا کمیسیون و شورا
    /// از این کنترل هم برای اعضا  شورا استفاده می شود و هم برای اعضا کمیسیون و 
    /// به صورت مشترک از یک جدول و سرویس استفاده می کنند
    /// البته نام ان باید تغییر کنند 
    /// </summary>

    [Authorize(Roles = "AdminUniversity")]
    public partial class SignersController : Controller
    {
        private readonly IBaseInfoComissionCouncilService _baseInfoComissionCouncilService;
        private readonly IRoleManagementService _roleManagementService;

        public SignersController(IBaseInfoComissionCouncilService baseInfoComissionCouncilService, IRoleManagementService roleManagementService)
        {
            _baseInfoComissionCouncilService = baseInfoComissionCouncilService;
            _roleManagementService = roleManagementService;
        }
        public virtual ActionResult Index(RequestType requestType)
        {
            var posts = (from a in _roleManagementService.GetAllPost().AsEnumerable()
                         select new DropDownModel { Value = a.Id.ToString(), Text = a.Name }).ToList();
            var postList = new SelectList(posts, "Value", "Text");
            ViewData["PostList"] = postList;

            var dropDownModelList = new List<DropDownModel>();
            for (int i = 1; i <= 8; i++)
            {
                dropDownModelList.Add(new DropDownModel(i.ToString(), i.ToString()));
            }
            var rowNumberList = new SelectList(dropDownModelList, "Value", "Text");
            ViewData["RowNumber"] = rowNumberList;
            ViewData["RequestType"] = requestType;
            if (requestType == RequestType.Comision)
                ViewData["RequestTypeText"] = "کمیسیون";
            else
                ViewData["RequestTypeText"] = "شورا";

            return View();
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult AddOrUpdateSigner(SignerModel signerModel)
        {
            try
            {
                if ((!ModelState.IsValid)||(signerModel.PostId<=0)||(signerModel.RowNumber<=0))
                    return Json(new { IsError = true, Message = "ورودی نا معتبر!" });
                var data = _baseInfoComissionCouncilService.AddOrUpdateSigner(signerModel, signerModel.Id != null && signerModel.Id > 0 ? StateOperation.ویرایش : StateOperation.درج);
                return Json(new { IsError = !data.Item1, Message = data.Item2 });
            }
            catch (Exception ex)
            {
                return Json(new { IsError = true, Message = "خطا در ثبت اطلاعات!" });
            }
        }

        public virtual ActionResult GetSigner(RequestType requestType)
        {
            try
            {
                var data = _baseInfoComissionCouncilService.WhereSigner(s => s.RequestType == requestType).ToList();

                var ja = new JArray();

                foreach (var itemObject in data.Select(item => new JObject
                {
                    {"Id", item.Id},
                    {"Name",item.Post.Name},
                    {"PostId",item.PostId},
                    {"RowNumber",item.RowNumber},
                    {"RequestType",item.RequestType.ToString()},
                    {"RequestTypeId",(int)item.RequestType}
                }))
                {
                    ja.Add(itemObject);
                }
                var jo = new JObject {{"total", data.ToList().Count}, {"rows", ja}};
                return Content(JsonConvert.SerializeObject(jo));
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, Msg = "خطا در لود اطلاعات پست" });
            }
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult DeleteSigner(long id)
        {
            try
            {
                var data = _baseInfoComissionCouncilService.DeleteSigner(id);
                return Json(new { IsError = !data.Item1, Message = data.Item2 });
            }
            catch (Exception)
            {
                return Json(new { IsError = true, Message = "خطا در حذف" });
            }
        }
    }
}