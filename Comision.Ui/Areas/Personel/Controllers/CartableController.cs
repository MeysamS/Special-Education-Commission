using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Helper.Utility;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Ui.Enums;
using Comision.Utility;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Personel.Controllers
{
    [AuthorizeUser]
    [Description("کارتابل امضا کنندگان")]
    [Expire]
    public partial class CartableController : Controller
    {
        private readonly ICommissionService _comissionService;
        private readonly ICartableService _cartableService;
        private readonly ITextDefaultService _textDefaultService;
        private readonly ISettingService _settingService;
        private readonly IRequestService _requestService;
        private readonly IAttachmentService _attachmentService;

        public CartableController(ICartableService cartableService, ITextDefaultService textDefaultService,
            ISettingService settingService, IRequestService requestService, IAttachmentService attachmentService, ICommissionService comissionService)
        {
            _cartableService = cartableService;
            _textDefaultService = textDefaultService;
            _settingService = settingService;
            _requestService = requestService;
            _attachmentService = attachmentService;
            _comissionService = comissionService;
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.ShowCartable, AllowInAccess = true)]
        public virtual ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.ShowCartable, AllowInAccess = false)]
        public virtual ActionResult GetListCartable()
        {
            try
            {
                var userLoginedId = Convert.ToInt64(User.Identity.GetUserId());
                var listCartable = _cartableService.GetListCartable(userLoginedId);
                if (listCartable.Item4 != null)
                    Elmah.ErrorSignal.FromCurrentContext().Raise(listCartable.Item4.InnerException);
                var ja = new JArray();
                foreach (var itemObject in listCartable.Item3.ToList().Select(item => new JObject
                {
                    {"Id", item.Request.Id},
                    {"RequestTypeId", (int) item.Request.RequestType},
                    {"RequestType", item.Request.RequestType.GetDescription()},
                    {
                        "CommissionNumber",
                        item.Commission?.CommissionNumber ?? item.Council.CouncilNumber
                    },
                    {"DateComission", item.Request.CreatedDate.ToPeString()},
                    {"RowNumber", item.Signer.RowNumber},//item.Signers.First().RowNumber
                    {"LastCommissionSigner", item.LastCommissionSigner},
                    {"LastCouncilSigner", item.LastCouncilSigner},
                    { "PostName", item.Signer.Post.Name},//item.Signers.First().Post.Name
                    {"RequestStatusId", (int) item.Request.RequestStatus},
                    {"RequestStatus", item.Request.RequestStatus.ToString()},
                    {"PersonId", item.Person.Id},
                    {"PostId", item.Signer.PostId},//item.Signers.First().PostId
                    {"StudentFullName", item.Person.Profile.FullName},
                    {"FieldofStudyName", item.OrganizationStructureName.Name},
                    {"Avatar", item.Person.Profile.Avatar??"profilepicture.png"},
                    {"StudentNumber", item.Person.Student.StudentNumber},
                    {"RelativeTimeCreateDate", RelativeTimeCalculator.Calculate(item.Request.CreatedDate)},
                    {"Description", item.Commission != null ?
                     (item.Commission.Description!=null? (item.Commission.Description.Length>100?item.Commission.Description.Substring(0,100)+"...":item.Commission.Description): ""):
                     (item.Council.Description!=null? (item.Council.Description.Length>100?item.Council.Description.Substring(0,100)+" ...":item.Council.Description): "")
                    },
                    {"FieldofStudyId",item.Student.FieldofStudyId }
                }))
                {
                    ja.Add(itemObject);
                }

                var jo = new JObject { { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            catch (Exception exception)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
                return Json(new { isError = true, Msg = "خطا در لود اطلاعات کارتابل" });
            }
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.ConfirmRequest, AllowInAccess = true)]
        public virtual ActionResult Confirm(long requestId, long postId, int rowNumber, string description, long fieldofStudyId)
        {
            var userInSigners = _cartableService.GetPersonInSigners(postId, fieldofStudyId);
            if (userInSigners.Item3 == null)
                return Json(new { isError = true, Message = userInSigners.Item2 });
            var userIdFrom = Convert.ToInt64(User.Identity.GetUserId());
            var result = _cartableService.Confirmation(userInSigners.Item3.Id, userIdFrom, requestId, postId, rowNumber, description);
            return Json(new { isError = !result.Item1, Message = result.Item2, ImgSigner = Url.Content("~/Content/Images/Signature/4a1c73a9-b65b-4442-9cfd-d9a26fd0fdb4.jpg") });
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.ConfirmRequestOnBehalfof, AllowInAccess = true)]
        public virtual ActionResult ConfirmOnBehalfof(long userId, long requestId, long postId, int rowNumber, string description)
        {
            var userIdFrom = Convert.ToInt64(User.Identity.GetUserId());
            var result = _cartableService.Confirmation(userId, userIdFrom, requestId, postId, rowNumber, description);
            var signature = new AddressUrlFile(Path.Combine("~\\")).Signature + result.Item3;

            return Json(new { isError = !result.Item1, Message = result.Item2, ImgSigner = signature });
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.ShowVote, AllowInAccess = true)]
        public virtual ActionResult Vote(long requestId, long postId, int rowNumber, RequestType requestType)
        {
            var levelId = Convert.ToInt64(User.LevelId());
            var userId = Convert.ToInt64(User.Identity.GetUserId());
            var data = _cartableService.GetVote(levelId,requestId, requestType, new AddressUrlFile(Path.Combine("~\\")));
            if (data.Item3 == null)
                return HttpNotFound();
            var voteModel = data.Item3;
            voteModel.PersonId = userId;
            voteModel.PersonIdOnBehalfof = userId;
            voteModel.PostId = postId;
            voteModel.RowNumber = rowNumber;

            var settings = _settingService.GetSettings();
            ViewBag.ordinal = (settings == null || settings.Ordinal == Ordinal.Ordinal);
            return View(voteModel);
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.VotePermanent, AllowInAccess = true)]
        public virtual ActionResult VotePermanent(VoteModel voteModel)
        {
            if (voteModel.RequestType == RequestType.Council)
                voteModel.ReferTo = true;
            var userInSigners = _cartableService.GetPersonInSigners(voteModel.RowNumber, voteModel.RequestType,
                voteModel.FieldofStudyId);
            if (userInSigners.Item3 == null)
                return Json(new { isError = true, Message = userInSigners.Item2 });
            var userIdFrom = Convert.ToInt64(User.Identity.GetUserId());
            voteModel.PersonIdOnBehalfof = userIdFrom;
            voteModel.PersonId = userInSigners.Item3.Id;
            if (!ModelState.IsValid)
                return Json(new { isError = true, Message = "ورودی نامعتبر!" });

            var result = _cartableService.VotePermanent(voteModel);
            return Json(new { isError = !result.Item1, Message = result.Item2 });
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.VoteTemporary, AllowInAccess = true)]
        public virtual ActionResult VoteTemporary(VoteModel voteModel)
        {
            if (voteModel.RequestType == RequestType.Council)
                voteModel.ReferTo = true;
            var userInSigners = _cartableService.GetPersonInSigners(voteModel.RowNumber, voteModel.RequestType,
                voteModel.FieldofStudyId);
            if (userInSigners.Item3 == null)
                return Json(new { isError = true, Message = userInSigners.Item2 });
            var userIdFrom = Convert.ToInt64(User.Identity.GetUserId());
            voteModel.PersonIdOnBehalfof = userIdFrom;
            voteModel.PersonId = userInSigners.Item3.Id;
            if (!ModelState.IsValid)
                return Json(new { isError = true, Message = "ورودی نامعتبر!" });

            var result = _cartableService.VoteTemporary(voteModel);
            return Json(new { isError = !result.Item1, Message = result.Item2 });
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.ReturnRequest, AllowInAccess = true)]
        public virtual ActionResult Return(long requestId, long postId, int rowNumber, string description, long fieldofStudyId)
        {
            var userInSigners = _cartableService.GetPersonInSigners(postId, fieldofStudyId);
            if (userInSigners.Item3 == null)
                return Json(new { isError = true, Message = userInSigners.Item2 });
            var userIdFrom = Convert.ToInt64(User.Identity.GetUserId());
            var result = _cartableService.Returned(userInSigners.Item3.Id, userIdFrom, requestId, postId, rowNumber, description);
            return Json(new { isError = !result.Item1, Message = result.Item2 });
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.AgreetoVote, AllowInAccess = true)]
        public virtual ActionResult AgreetoVote(long requestId, long postId, int rowNumber, string description, long fieldofStudyId)
        {
            var userInSigners = _cartableService.GetPersonInSigners(postId, fieldofStudyId);
            if (userInSigners.Item3 == null)
                return Json(new { isError = true, Message = userInSigners.Item2 });
            var userIdFrom = Convert.ToInt64(User.Identity.GetUserId());
            var result = _cartableService.AgreetoVote(userInSigners.Item3.Id, userIdFrom, requestId, postId, rowNumber, description);
            return Json(new { isError = !result.Item1, Message = result.Item2 });
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.OppositiontoVote, AllowInAccess = true)]
        public virtual ActionResult OppositiontoVote(long requestId, long postId, int rowNumber, string description, long fieldofStudyId)
        {
            var userInSigners = _cartableService.GetPersonInSigners(postId, fieldofStudyId);
            if (userInSigners.Item3 == null)
                return Json(new { isError = true, Message = userInSigners.Item2 });
            var userIdFrom = Convert.ToInt64(User.Identity.GetUserId());
            var result = _cartableService.OppositiontoVote(userInSigners.Item3.Id, userIdFrom, requestId, postId, rowNumber, description);
            return Json(new { isError = !result.Item1, Message = result.Item2 });
        }

        public string GetCartableCount()
        {
            var userId = Convert.ToInt64(User.Identity.GetUserId());
            return Convert.ToString(_cartableService.GetCartableCount(userId));
        }

        public virtual JsonResult GetCartableRequestCount(long userId)
        {
            //var userId = Convert.ToInt64(User.Identity.GetUserId());
            var requestCount = _requestService.GetRequestCount(userId);
            var cartableCount = _cartableService.GetCartableCount(userId);
            return Json(new { RequestCount = requestCount, CartableCount = cartableCount });
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCartableShow, AllowInAccess = true)]
        public virtual ActionResult Attachment(long requestId)
        {
            ViewBag.RequestId = requestId;
            ViewBag.ControllerName = ControllerName.Cartable;
            return PartialView(MVC.Personel.Cartable.Views._Attachment);
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCartableShow, AllowInAccess = false)]
        public virtual ActionResult GetAttachment(long requestId)
        {
            try
            {
                var listAttach = _attachmentService.GetAll(requestId).ToList();
                var ja = new JArray();
                foreach (var item in listAttach)
                {
                    var itemObject = new JObject
                    {
                        {"id",item.Id},
                        {"title", item.Title},
                        {"file",item.File },
                        {"extention",item.Extention},
                        {"size",item.Size },
                        {"requestId",item.RequestId }
                    };
                    ja.Add(itemObject);
                }

                var jo = new JObject { { "total", listAttach.Count }, { "rows", ja } };
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            catch (Exception exception)
            {
                return Json(new { isError = true, Msg = "خطا در لود اطلاعات پیوست" });
            }
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCartableUpload, AllowInAccess = true)]
        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult UploadAttachment(AttachmentModel attachmentModel, HttpPostedFileBase fileAttach)
        {
            try
            {
                if (fileAttach == null)
                {
                    return Json(new { isError = true, Message = @"ورودی نامعتبر!" });
                }
                attachmentModel.Extention = Path.GetExtension(fileAttach.FileName);
                attachmentModel.Size = fileAttach.ContentLength;
                if (!ModelState.IsValid)
                {
                    return Json(new { isError = true, Message = @"ورودی نامعتبر!" });
                }
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var addressUrlFile = new AddressUrlFile(Server.MapPath("~//"));
                    var path = addressUrlFile.UploadFiles;
                    Useful.CreateFolderIfNeeded(addressUrlFile.UploadFiles);
                    path += Guid.NewGuid() + Path.GetExtension(fileAttach.FileName);
                    fileAttach.SaveAs(path);
                    attachmentModel.File = Path.GetFileName(path);

                    var data = _attachmentService.Add(attachmentModel);
                    scope.Complete();
                    return Json(new { isError = !data.Item1, Message = data.Item2 });
                }
            }
            catch (Exception)
            {
                return Json(new { isError = true, Message = @"خطا در ویرایش اطلاعات پرسنلی" });
            }
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCartableDelete, AllowInAccess = true)]
        [HttpPost]
        public virtual ActionResult DeleteFile(long id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var mypatfile = new AddressUrlFile(Path.Combine("~\\"));
                    var attach = _attachmentService.Find(id);
                    if (attach == null)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    string fullPath = Request.MapPath(mypatfile.UploadFiles + attach.File + attach.Extention);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    _attachmentService.Delete(id);
                    scope.Complete();
                    return Json(new { isError = false, Message = "حذف انجام شد" });
                }
            }
            catch (Exception exception)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
                return Json(new { isError = true, Message = "خطا در حذف پیوست" });
            }
        }
    }
}