using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Helper.Utility;
using Comision.Model.Domain;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;

namespace Comision.Ui.Areas.Admin.Controllers
{
    public partial class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }
        // GET: Admin/Slider
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult GetSliders()
        {
            var model = _sliderService.GetListSlider().ToList();
            return PartialView("_GetSliders", model);
        }


        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult RemoveSlider(int sliderId)
        {
            var slider = _sliderService.Find(sliderId);
            if (slider == null)
            {
                return Json(new { Success = false, Msg = "فایل مورد نظر یافت نشد" });
            }
            if (!_sliderService.Delete(sliderId).Item1) return Json(new {Success = false, Msg = "خطا در حذف"});
            var addressUrlFile = new AddressUrlFile(Server.MapPath("~//"));
            var path = addressUrlFile.SliderImage + slider.ImageName;
            System.IO.File.Delete(path);
            return Json(new { Success = true, Msg = "تصویر حذف شد" });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Create(IEnumerable<HttpPostedFileBase> sliderUpload)
        {
            if (sliderUpload == null)
            {
                return Json(new { Success = false, Msg = "تصویر وارد نشده است" });
            }
            if (!ModelState.IsValid) return Json(new { Success = false, Msg = ModelState.GetErrors() });
            var addressUrlFile = new AddressUrlFile(Server.MapPath("~//"));
            Useful.CreateFolderIfNeeded(addressUrlFile.SliderImage);
            var httpPostedFileBases = sliderUpload as HttpPostedFileBase[] ?? sliderUpload.ToArray();
            for (var i = 0; i < httpPostedFileBases.Count(); i++)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(httpPostedFileBases[i].FileName);
                var slider = new Slider { ImageName = fileName };
                httpPostedFileBases[i].SaveAs(addressUrlFile.SliderImage + fileName);
                _sliderService.Add(slider);
            }
            return Json(new { Success = true, Msg = "رکورد جدید برای اسلایدر ثبت شد", Html = this.RenderPartialToString("~/Areas/Admin/Views/Slider/_GetSliders.cshtml", _sliderService.GetListSlider()) });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Delete(int id)
        {
            try
            {
                _sliderService.Delete(id);
                return Json(new { success = true, Msg = "حذف شد" });
            }
            catch (Exception)
            {

                return Json(new { success = false, Msg = "حذف نشد" });
            }
        }

        [HttpGet]
        [AjaxOnly]
        public virtual JsonResult GetImagesSlider()
        {
            try
            {
                var model = _sliderService.GetListSliderPath(new AddressUrlFile(Path.Combine("\\")));
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Error = true });
            }
        }

    }
}