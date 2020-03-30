using System;
using System.Data.Entity.Validation;
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
    public partial class PostController : Controller
    {
        private readonly IRoleManagementService _roleManagementService;
        public PostController(IRoleManagementService roleManagementService)
        {
            _roleManagementService = roleManagementService;
        }

        public virtual ActionResult Index()
        {
            var enumPostType = EnumerationService.GetEnumValues<PostType>();
            enumPostType.RemoveRange(0,2);
            var enumPostTypeList = new SelectList(enumPostType, "Value", "Text");
            ViewData["EPostType"] = enumPostTypeList;
            return View();
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult AddOrUpdate(PostModel postModel)
        {
            try
            {
                Tuple<bool, string> state;
                ModelState.Remove("Id");
                if (!ModelState.IsValid)
                    return Json(new { isError = true, Msg = "ورودی نا معتبر!" });
                if (postModel.Id == null|| postModel.Id==0)
                    state = _roleManagementService.AddOrUpdatePost(postModel, StateOperation.درج);
                else
                    state = _roleManagementService.AddOrUpdatePost(postModel, StateOperation.ویرایش);

                return Json(new { isError = !state.Item1, Msg = state.Item2 });
            }
            catch (DbEntityValidationException)
            {
                return Json(new { isError = true, Msg = "خطا در ثبت اطلاعات!" });
            }
        }

        public virtual ActionResult GetPost()
        {
            try
            {
                 var data = _roleManagementService.GetAllPost();
                JArray ja = new JArray();

                foreach (var item in data)
                {
                    var itemObject = new JObject
                    {
                        {"Id", item.Id},
                        {"Name",item.Name},
                        {"PostType",item.PostType.GetDescription()},
                        {"PostTypeId",(int)item.PostType}
                    };
                    ja.Add(itemObject);
                }
                JObject jo = new JObject();
                jo.Add("total", _roleManagementService.CountPosts());
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
            var state = _roleManagementService.DeletePost(id);
                return Json(new { isError = !state.Item1, Msg = state.Item2 });

        }

    }
}