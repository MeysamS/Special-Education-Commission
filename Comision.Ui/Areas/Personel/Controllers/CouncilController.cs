using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Enum;
using Comision.Service.ImplementationService;
using Comision.Service.IService;
using Comision.Service.Model;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Net;
using Comision.Ui.Areas.Personel.Models;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Transactions;
using System.Web;
using Comision.Model.Domain;
using Comision.Service.Enum;
using Comision.Ui.Enums;
using Comision.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Personel.Controllers
{
    [AuthorizeUser]
    [Description("طرح در شورای آموزشی")]
    public partial class CouncilController : Controller
    {
        private readonly ICouncilService _councilService;
        private readonly IStructureManageService _structureManageService;
        private readonly ICartableService _cartableService;
        private readonly IBaseInfoComissionCouncilService _baseInfoComissionCouncilService;
        private readonly IPersonManagementService _personManagementService;
        private readonly IAttachmentService _attachmentService;
        private readonly ITextDefaultService _textDefaultService;
        public CouncilController(ICouncilService councilService,
            IStructureManageService structureManageService, ICartableService cartableService,
            IBaseInfoComissionCouncilService baseInfoComissionCouncilService, IPersonManagementService personManagementService,
            IAttachmentService attachmentService, ITextDefaultService textDefaultService)
        {
            _councilService = councilService;
            _structureManageService = structureManageService;
            _cartableService = cartableService;
            _baseInfoComissionCouncilService = baseInfoComissionCouncilService;
            _personManagementService = personManagementService;
            _attachmentService = attachmentService;
            _textDefaultService = textDefaultService;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult Delete(long id)
        {
            var sate = _textDefaultService.Delete(id);
            return Json(new { isError = false, Msg = sate.Item2 });
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.CreateCouncil, AllowInAccess = true)]
        [HttpGet]
        public virtual ActionResult Create()
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var enumGrade = EnumerationService.GetEnumValues<Grade>();
            var enumGradeList = new SelectList(enumGrade, "Value", "Text");
            ViewData["Grade"] = enumGradeList;

            var enumMilitaryServiceStatus = EnumerationService.GetEnumValues<MilitaryServiceStatus>();
            var enumMilitaryServiceStatusList = new SelectList(enumMilitaryServiceStatus, "Value", "Text");
            ViewData["MilitaryServiceStatus"] = enumMilitaryServiceStatusList;

            var querysearch = _structureManageService.GetAllFieldofStudy(userId, 1).Item3.AsEnumerable();
            if (querysearch != null)
            {
                var fieldofStudies = (from f in querysearch
                                      select new DropDownModel { Value = f.Id.ToString(), Text = f.OrganizationStructureName.Name }).ToList();
                var listFieldofStudy = new SelectList(fieldofStudies, "Value", "Text");
                ViewData["FieldofStudy"] = listFieldofStudy;
            }
            else ViewData["FieldofStudy"] = new List<DropDownModel>();


            var gender = EnumerationService.GetEnumValues<Gender>();
            var genderList = new SelectList(gender, "Value", "Text");
            TempData["Gender"] = genderList;

            var userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
            // این متد به کنترل صدور رای منتقل شده است
            // در فرم صدور رای نیاز به شماره کمیسیون می باشد
            ////var levelId = Convert.ToInt64(User.LevelId());
            ////var councilNumber = _councilService.GetCouncilNumber(levelId);
            return View(new CouncilModel { CouncilNumber =0, Date = DateTime.Now });
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.CreateCouncil, AllowInAccess = false)]
        public virtual ActionResult Create(CouncilModel councilModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return Json(new { IsError = true, Message = "ورودی نامعتبر1!" });
            //var userId = long.Parse(User.Identity.GetUserId());
            //var data = _personManagementService.IsStudentExist(councilModel.StudentNumber, userId,
            //            councilModel.RequestType).Result;
            //if (!data.Item1)
            //    return Json(new { isError = true, Message = data.Item2 });

            var userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
            var levelId = Convert.ToInt64(User.LevelId());
            councilModel.RequestType = RequestType.Council;
            councilModel.RequestStatus = RequestStatus.Waiting;
            var result = _councilService.AddCouncilRequest(councilModel, levelId);
            return Json(new { IsError = !result.Item1, Message = result.Item2, requestId = result.Item3 });
        }

        [HttpGet]
        [CustomAuthorize(RoleNameDefault = AccessLevel.UpdateCouncil, AllowInAccess = true)]
        public virtual ActionResult Update(long requestId)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var councilModel = _councilService.FindInfoCouncil(requestId);

            var enumGrade = EnumerationService.GetEnumValues<Grade>();
            var enumGradeList = new SelectList(enumGrade, "Value", "Text");
            TempData["Grade"] = enumGradeList;

            var enumMilitaryServiceStatus = EnumerationService.GetEnumValues<MilitaryServiceStatus>();
            var enumMilitaryServiceStatusList = new SelectList(enumMilitaryServiceStatus, "Value", "Text");
            TempData["MilitaryServiceStatus"] = enumMilitaryServiceStatusList;

            var querysearch = _structureManageService.GetAllFieldofStudy(userId, 1).Item3.AsEnumerable();
            if (querysearch != null)
            {
                var fieldofStudies = (from f in querysearch
                                      select new DropDownModel { Value = f.Id.ToString(), Text = f.OrganizationStructureName.Name }).ToList();
                var listFieldofStudy = new SelectList(fieldofStudies, "Value", "Text");
                TempData["FieldofStudy"] = listFieldofStudy;
            }
            else
                TempData["FieldofStudy"] = new List<DropDownModel>();

            var gender = EnumerationService.GetEnumValues<Gender>();
            var genderList = new SelectList(gender, "Value", "Text");
            TempData["Gender"] = genderList;

            return View(councilModel.Item3);
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.UpdateCouncil, AllowInAccess = false)]
        public virtual ActionResult Update(CouncilModel councilModel)
        {
            if (!ModelState.IsValid)
                return Json(new { IsError = true, Message = "ورودی نامعتبر!" });
            var userId = long.Parse(User.Identity.GetUserId());
            //var data = _personManagementService.IsStudentExist(councilModel.StudentNumber, userId,
            //            councilModel.RequestType).Result;
            //if (!data.Item1)
            //    return Json(new { isError = true, Message = data.Item2 });

            var userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
            var levelId = Convert.ToInt64(User.LevelId());
            var result = _councilService.UpdateCouncilRequest(councilModel);
            return Json(new { IsError = !result.Item1, Message = result.Item2 });
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.SendCouncilToCartable, AllowInAccess = true)]
        public virtual ActionResult SendCouncilToCartable(long requestId, int rowNumber, string description, long fieldofStudyId)
        {
            //var userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
            //var levelId = Convert.ToInt64(User.LevelId());
            var signer = _baseInfoComissionCouncilService.WhereSigner(s => s.RowNumber == rowNumber && s.RequestType == RequestType.Council).FirstOrDefault();
            var postId = signer?.PostId ?? 0;
            var userInSigners = _cartableService.GetPersonInSigners(postId, fieldofStudyId);
            if (userInSigners.Item3 == null)
                return Json(new { isError = true, Message = userInSigners.Item2 });
            var userIdFrom = Convert.ToInt64(User.Identity.GetUserId());
            var result = _cartableService.Confirmation(userInSigners.Item3.Id, userIdFrom, requestId, postId, rowNumber, description);
            return Json(new { isError = !result.Item1, Message = result.Item2 });
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCouncilShow, AllowInAccess = true)]
        public virtual ActionResult Attachment(long requestId)
        {
            ViewBag.RequestId = requestId;
            ViewBag.ControllerName = ControllerName.Council;
            return PartialView(MVC.Personel.Cartable.Views._Attachment);
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCouncilShow, AllowInAccess = false)]
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

        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCouncilUpload, AllowInAccess = true)]
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

        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCouncilDelete, AllowInAccess = true)]
        [HttpPost]
        public virtual ActionResult DeleteFile(long id)
        {
            try
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
                return Json(new { isError = false, Message = "حذف انجام شد" });
            }
            catch (Exception)
            {
                return Json(new { isError = true, Message = "خطا در حذف پیوست" });
            }

        }
        
        //[CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCommissionUpload, AllowInAccess = true)]
        //[HttpPost]
        //public virtual ActionResult SaveUploadFiles(long requestId)
        //{
        //    bool isSavedSuccessfully = true;
        //    string fName = "";
        //    try
        //    {
        //        var mypatfile = new AddressUrlFile(Server.MapPath("~\\"));
        //        foreach (string fileName in Request.Files)
        //        {
        //            HttpPostedFileBase file = Request.Files[fileName];
        //            //Save file content goes here
        //            if (file != null)
        //            {
        //                fName = file.FileName;
        //                if (file.ContentLength > 0)
        //                {
        //                    // var originalDirectory = new DirectoryInfo(string.Format("{0}UploadFiles\\Comision\\", Server.MapPath(@"\")));
        //                    var originalDirectory = new DirectoryInfo(mypatfile.UploadFiles);
        //                    string pathString = Path.Combine(originalDirectory.ToString());
        //                    var fileName1 = Path.GetFileName(file.FileName);
        //                    bool isExists = Directory.Exists(pathString);
        //                    if (!isExists)
        //                        Directory.CreateDirectory(pathString);
        //                    var path = string.Format("{0}\\{1}", pathString, Guid.NewGuid() + Path.GetExtension(file.FileName));
        //                    file.SaveAs(path);
        //                    FileInfo f = new FileInfo(path);
        //                    var model = new AttachmentModel { RequestId = requestId, File = Path.GetFileNameWithoutExtension(path), Extention = Path.GetExtension(path), Size = f.Length };
        //                    _attachmentService.Add(model);
        //                }
        //            }
        //        }
        //        //_unitOfWork.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        isSavedSuccessfully = false;
        //    }


        //    if (isSavedSuccessfully)
        //    {
        //        return Json(new { Message = fName });
        //    }
        //    else
        //    {
        //        return Json(new { Message = "Error in saving file" });
        //    }
        //}

        //[CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCommissionShow, AllowInAccess = false)]
        //[HttpGet]
        //public virtual ActionResult GetAttachments(long requestId)
        //{
        //    var attachments = _attachmentService.GetAll(requestId).ToList();
        //    //این متد بعد از کامیت با مشکل مواجه شده بود برای همین  موقتا غیر فعال شده است

        //    var mypatfile = new AddressUrlFile(Path.Combine("~\\"));
        //    List<AttachmentListModel> model = attachments.Select(item => new AttachmentListModel
        //    {
        //        AttachmentId = item.Id,
        //        RequestId = item.RequestId,
        //        FileName = item.File + item.Extention,
        //        Extention = item.Extention,
        //        Path = mypatfile.UploadFiles + item.File + item.Extention,
        //        Size = item.Size
        //    }).ToList();

        //    return Json(new { Data = model }, JsonRequestBehavior.AllowGet);
        //}
    }
}