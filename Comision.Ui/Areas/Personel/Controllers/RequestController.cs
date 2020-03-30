using System;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Helper.Utility;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Transactions;

namespace Comision.Ui.Areas.Personel.Controllers
{
    [AuthorizeUser]
    [Description("کارتابل کارشناس")]
    [Expire]
    public partial class RequestController : Controller
    {
        // GET: Personel/Request

        private readonly IRequestService _requestService;
        private readonly ISignerRepository _signerRepository;
        private readonly ICartableRepository _cartableRepository;
        private readonly IBaseInfoComissionCouncilService _baseInfoComissionCouncilService;
        private readonly ISettingService _settingService;
        private readonly IAttachmentRepository _attachmentRepository;

        public RequestController(IRequestService requestService, ISignerRepository signerRepository,
            ICartableRepository cartableRepository, IBaseInfoComissionCouncilService baseInfoComissionCouncilService,
            ISettingService settingService, IAttachmentRepository attachmentRepository)
        {
            _requestService = requestService;
            _signerRepository = signerRepository;
            _cartableRepository = cartableRepository;
            _baseInfoComissionCouncilService = baseInfoComissionCouncilService;
            _settingService = settingService;
            _attachmentRepository = attachmentRepository;
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.ShowCartableRequest, AllowInAccess = true)]
        public virtual ActionResult Index()
        {
            ViewBag.rowNumber = 1;
            return View();
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.DetailRequest, AllowInAccess = true)]
        public virtual ActionResult DetailRequest(long requestId, long postId, long rowNumber, int lastCommissionSigner, int lastCouncilSigner, string rootUrl)
        {
            var detailRequestModel = _requestService.DetailRequest(requestId, new AddressUrlFile(Path.Combine("~\\"))).Item3;
            var settings = _settingService.GetSettings();
            ViewBag.ordinal = (settings == null || settings.Ordinal == Ordinal.Ordinal);
            ViewBag.rootUrl = rootUrl;
            ViewBag.postId = postId;
            ViewBag.rowNumber = rowNumber;
            ViewBag.lastCommissionSigner = lastCommissionSigner;
            ViewBag.lastCouncilSigner = lastCouncilSigner;
            return View(MVC.Personel.Request.Views.DetailRequest, detailRequestModel);
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.DetailRequest, AllowInAccess = false)]
        public virtual ActionResult DetailRequestPartial(long requestId)
        {
            var detailRequestModel = _requestService.DetailRequest(requestId, new AddressUrlFile(Path.Combine("~\\"))).Item3;
            var settings = _settingService.GetSettings();
            ViewBag.ordinal = (settings == null || settings.Ordinal == Ordinal.Ordinal);
            return PartialView(MVC.Personel.Request.Views._DetailRequest, detailRequestModel);
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.DetailRequest + "," + AccessLevel.DetailRequestPartial, AllowInAccess = false)]
        public virtual ActionResult GetListSpecialEducationforRequest(long requestId)
        {
            try
            {
                var comision = _baseInfoComissionCouncilService.GetAllSpecialEducationforRequest(requestId);
                var ja = comision.Select(item => new JObject
                {
                    {"value", item.Id},
                    { "text", item.Name}
                    //{"checked",comision.Any(c=>c.Id== item.Id)}
                }).ToList();
                return Content(JsonConvert.SerializeObject(ja), "application/json");
            }
            catch (Exception)
            {
                return Json(new { isError = true, Message = "خطا در بارگزاری درخواست ها!" });
            }
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.GetListRequest, AllowInAccess = true)]
        public virtual ActionResult GetListRequest()
        {
            try
            {
                var userId = Convert.ToInt64(User.Identity.GetUserId());
                var requests = _requestService.GetListRequest(userId).Item3;
                var ja = new JArray();
                foreach (var itemObject in requests.Select(item => new JObject
                {
                    {"Id",item.Id},
                    {"PersonId",item.PersonId},
                    {"Avatar",item.Person.Profile.Avatar},
                    {"NameFamily",item.Person.Profile.Name+" "+item.Person.Profile.Family},
                    {"StudentNumber",item.Person.Student.StudentNumber },
                    {"FieldofStudy",item.Person.Student.FieldofStudy.OrganizationStructureName.Name},
                    {"FieldofStudyId",item.Person.Student.FieldofStudyId},
                    {"CreateDateReleative",RelativeTimeCalculator.Calculate(item.Commission?.Date ?? item.Council.Date)},
                    {"CreateDate",item.Commission?.Date.ToPeString() ?? item.Council.Date.ToPeString()},
                    {"RequestTypeId",(int)item.RequestType },
                    {"RequestStatusId",(int)item.RequestStatus },
                    {"RequestType",item.RequestType.GetDescription()},
                    {"Number",item.Commission?.CommissionNumber ?? item.Council.CouncilNumber},
                     {"Description", item.Commission != null ?
                     (item.Commission.Description!=null? (item.Commission.Description.Length>100?item.Commission.Description.Substring(0,100)+"...":item.Commission.Description): ""):
                     (item.Council.Description!=null? (item.Council.Description.Length>100?item.Council.Description.Substring(0,100)+" ...":item.Council.Description): "")
                    },
                    {"VoteTypeId", item.Vote==null?0: (int)item.Vote.VoteType}
                }))
                {
                    ja.Add(itemObject);
                }
                JObject jo = new JObject { { "total", requests.Count }, { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo));
            }
            catch (Exception)
            {
                return Json(new { isError = true, Message = "خطا در بارگزاری درخواست ها!" });
            }
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.FollowUpRequest, AllowInAccess = true)]
        public virtual ActionResult FollowUp()
        {
            return View();
        }


        [CustomAuthorize(RoleNameDefault = AccessLevel.FollowUpRequest, AllowInAccess = false)]
        [HttpGet]
        [AjaxOnly]
        public virtual ActionResult GetRequestByStudentCode(long stNumber)
        {
            try
            {
                var model = _requestService.GetRequestByStudentCode(stNumber).ToList();
                return PartialView("_FollowRequests", model);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, Message = "خطا در بارگزاری درخواست ها!" });
            }
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.FollowUpRequest, AllowInAccess = false)]
        [HttpGet]
        [AjaxOnly]
        public virtual ActionResult GetFollowByRequestId(long reqId)
        {
            var model = _cartableRepository.Where(x => x.RequestId == reqId)
                .Include(x => x.Person)
                .Include(x => x.Person.Profile)
                .Include(x => x.Request)
                .Include(x => x.Post)
                .Include(x => x.Request.Commission)
                .Include(x => x.Request.Council)
                .ToList();
            return PartialView("_FollowUpDetail", model);
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.ArchiveRequest, AllowInAccess = true)]
        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Archive(long requestId)
        {
            var userId = Convert.ToInt64(User.Identity.GetUserId());
            var result = _requestService.Archive(requestId, userId, DateTime.Today.Date);
            return Json(new { isError = !result.Item1, Message = result.Item2 });
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.DeleteRequest, AllowInAccess = true)]
        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult DeleteRequest(long requestId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var attach = _attachmentRepository.Where(a => a.RequestId == requestId).Select(s => s.File).ToList();
                    var addressUrlFile = new AddressUrlFile(Server.MapPath("~//"));
                    var result = _requestService.Delete(requestId);
                    if (result.Item1)
                    {
                        for (int i = 0; i < attach.Count; i++)
                        {
                            if (System.IO.File.Exists(addressUrlFile.UploadFiles + attach[i]))
                                System.IO.File.Delete(addressUrlFile.UploadFiles + attach[i]);
                        }
                        scope.Complete();
                    }
                    return Json(new { isError = !result.Item1, Message = result.Item2 });
                }
            }
            catch (Exception)
            {
                return Json(new { isError = true, Message = "عملیات حذف با مشکل مواجه شده است" });
            }
        }
        public string GetRequestCount()
        {

            var userId = Convert.ToInt64(User.Identity.GetUserId());
            return Convert.ToString(_requestService.GetRequestCount(userId));
        }
    }
}