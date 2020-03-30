using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Domain;
using Comision.Service.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Newtonsoft.Json;

namespace Comision.Ui.Areas.Admin.Controllers
{
    [Authorize(Roles = "AdminUniversity")]
    public partial class StructureController : Controller
    {
        // GET: Admin/Structure
        private readonly IStructureManageService _structureManageService;
        public StructureController(IStructureManageService structureManageService)
        {
            _structureManageService = structureManageService;
        }
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult GetStructures()
        {
            return Content(JsonConvert.SerializeObject(_structureManageService.GetStructureByTreeView(1)));
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Create(StructureTreeModel model)
        {
            //var error = ModelState.Values.SelectMany(x => x.Errors).Select(c => c.ErrorMessage).ToList();
            if (!ModelState.IsValid)
                return Json(new { IsError = true, Message = "ورودی نامعتبر" });

            var result = _structureManageService.AddorUpdateStructureComplete(model);
            return Json(new { IsError = !result.Item1, Message = result.Item2 });
        }


        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult EditName(StructureTreeModel model)
        {
            if (!ModelState.IsValid || model.OrgId == null)
                return Json(new { IsError = true, Message = "ورودی نامعتبر" });

            var org = new OrganizationStructureName
            { Id = (long)model.OrgId, Name = model.Text, StructureType=model.StructureType };
            var result = _structureManageService.AddOrUpdateStructure(org, StateOperation.ویرایش);
            return Json(new { IsError = !result.Item1, Message = result.Item2 });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Delete(StructureTreeModel model)
        {
            ModelState.Remove("Text");
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.Id.ToString()) || string.IsNullOrEmpty(model.OrgId.ToString()))
                return Json(new { IsError = true, Message = "ورودی نامعتبر" });

            var result = _structureManageService.Delete(model);
            return Json(new { IsError = !result.Item1, Message = result.Item2 });
        }



    }
}