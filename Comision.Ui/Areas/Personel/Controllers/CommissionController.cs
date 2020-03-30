using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Comision.Data.Context;
using Comision.Helper.Filters;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.ImplementationService;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Ui.Areas.Personel.Models;
using Comision.Ui.Enums;
using Comision.Utility;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Comision.Ui.Areas.Personel.Controllers
{
    [AuthorizeUser]
    [Description("کمیسیون موارد خاص")]
    public partial class CommissionController : Controller
    {
        private readonly ICommissionService _comissionService;
        private readonly IBaseInfoComissionCouncilService _baseInfoComissionCouncilService;
        private readonly IStructureManageService _structureManageService;
        private readonly ICartableService _cartableService;
        private readonly IAttachmentService _attachmentService;
        private readonly IPersonManagementService _personManagementService;
        private readonly IUnitOfWork _unitOfWork;

        public CommissionController(IUnitOfWork unitOfWork, ICommissionService comissionService,
            IBaseInfoComissionCouncilService baseInfoComissionCouncilService,
            IStructureManageService structureManageService, ICartableService cartableService,
            IAttachmentService attachmentService, IPersonManagementService personManagementService)
        {
            _comissionService = comissionService;
            _baseInfoComissionCouncilService = baseInfoComissionCouncilService;
            _structureManageService = structureManageService;
            _cartableService = cartableService;
            _attachmentService = attachmentService;
            _personManagementService = personManagementService;
            _unitOfWork = unitOfWork;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.CreateCommission + "," + AccessLevel.UpdateCommission)]
        public virtual ActionResult GetListSpecialEducation()
        {
            try
            {
                var specialEducation = _baseInfoComissionCouncilService.GetAllSpecialEducation();

                var ja = specialEducation.Select(item => new JObject
                {
                    {"value", item.Id},
                    { "text", item.Name}
                }).ToList();
                //JObject jo = new JObject {{"total", specialEducation.Count}, {"rows", ja}};
                return Content(JsonConvert.SerializeObject(ja), "application/json");
            }
            catch (Exception exception)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
                return Json(new { isError = true, Message = "خطا در بارگزاری درخواست ها!" });
            }
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.UpdateCommission, AllowInAccess = false)]
        public virtual ActionResult GetListSpecialEducationforRequest(long requestId)
        {
            try
            {
                var specialEducation = _baseInfoComissionCouncilService.GetAllSpecialEducation();
                var comision = _baseInfoComissionCouncilService.GetAllSpecialEducationforRequest(requestId);
                var ja = specialEducation.Select(item => new JObject
                {
                    {"value", item.Id},
                    { "text", item.Name},
                    {"checked",comision.Any(c=>c.Id== item.Id)}
                }).ToList();
                //JObject jo = new JObject {{"total", specialEducation.Count}, {"rows", ja}};
                return Content(JsonConvert.SerializeObject(ja), "application/json");
            }
            catch (Exception exception)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
                return Json(new { isError = true, Message = "خطا در بارگزاری درخواست ها!" });
            }
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.CreateCommission, AllowInAccess = true)]
        [HttpGet]
        public virtual ActionResult Create()
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var enumGrade = EnumerationService.GetEnumValues<Grade>();
            var enumGradeList = new SelectList(enumGrade, "Value", "Text");
            ViewData["Grade"] = enumGradeList;

            //var Numberitem = _comissionService.GetCommissionNumber(1);
            //ViewData["CommissionNumber"] = Numberitem.Item1;

            var gender = EnumerationService.GetEnumValues<Gender>();
            var genderList = new SelectList(gender, "Value", "Text");
            TempData["Gender"] = genderList;

            var enumMilitaryServiceStatus = EnumerationService.GetEnumValues<MilitaryServiceStatus>();
            var enumMilitaryServiceStatusList = new SelectList(enumMilitaryServiceStatus, "Value", "Text");
            ViewData["MilitaryServiceStatus"] = enumMilitaryServiceStatusList;

            // این اکد اشتباه می باشد زیرا ممکن است رشته ای برای کاربر در نظر گرفته نشده باشد
            // برای همین خطا خواهیم داشت و چه طوری باید مقادیر در 
            // drowpfownmoel
            // قرار بگیرد
            var queryItemFieldofStudy = _structureManageService.GetAllFieldofStudy(userId, 1).Item3.AsEnumerable();
            if (queryItemFieldofStudy != null)
            {
                var fieldofStudies = (from f in queryItemFieldofStudy
                                      select new DropDownModel { Value = f.Id.ToString(), Text = f.OrganizationStructureName.Name }).ToList();
                var listFieldofStudy = new SelectList(fieldofStudies, "Value", "Text");
                ViewData["FieldofStudy"] = listFieldofStudy;
            }
            else                
            ViewData["FieldofStudy"] = new List<DropDownModel>();
            var userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);

            // این متد در اینجا برداشته شده است و به فرم صدور رای منتقل شده است زیرا در صدور رای باید شماره کمیسیون زده شود
            ////var levelId = Convert.ToInt64(User.LevelId());
            ////var commissionNumber = _comissionService.GetCommissionNumber(levelId);
            return View(new CommissionModel { CommissionNumber =0,Date = DateTime.Now});
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.CreateCommission)]
        public virtual ActionResult Create(string special, CommissionModel commissionModel)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return Json(new { isError = true, Message = "ورودی نامعتبر!" });
            //var userId = long.Parse(User.Identity.GetUserId());
            //var data = _personManagementService.IsStudentExist(commissionModel.StudentNumber, userId,
            //            commissionModel.RequestType).Result;
            //if (!data.Item1)
            //    return Json(new { isError = true, Message = data.Item2 });

            string[] arrSpecialEducations = special.Split('&');
            if (arrSpecialEducations.Length > 1)
            {
                string[] fields;
                long specialId;

                for (var i = 0; i < arrSpecialEducations.Length - 1; i++)
                {
                    fields = arrSpecialEducations[i].Split(',');
                    specialId = Convert.ToInt64(fields[0]);
                    CommissionSpecialEducation commissionSpecialEducation = new CommissionSpecialEducation
                    {
                        // CommissionId = Convert.ToInt64(commissionModel.Id),
                        SpecialEducationId = specialId
                    };
                    commissionModel.CommissionSpecialEducations.Add(commissionSpecialEducation);
                }
            }
            var userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
            var levelId = Convert.ToInt64(User.LevelId());
            commissionModel.RequestType = RequestType.Comision;
            commissionModel.RequestStatus = RequestStatus.Waiting;
            var result = _comissionService.AddCommissionRequest(commissionModel, levelId);
            return Json(new { isError = !result.Item1, Message = result.Item2, requestId = result.Item3 });
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.UpdateCommission, AllowInAccess = true)]
        [HttpGet]
        public virtual ActionResult Update(long requestId)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var commissionModel = _comissionService.FindInfoCommission(requestId);

            var enumGrade = EnumerationService.GetEnumValues<Grade>();
            var enumGradeList = new SelectList(enumGrade, "Value", "Text");
            TempData["Grade"] = enumGradeList;

            var enumMilitaryServiceStatus = EnumerationService.GetEnumValues<MilitaryServiceStatus>();
            var enumMilitaryServiceStatusList = new SelectList(enumMilitaryServiceStatus, "Value", "Text");
            TempData["MilitaryServiceStatus"] = enumMilitaryServiceStatusList;

            var gender = EnumerationService.GetEnumValues<Gender>();
            var genderList = new SelectList(gender, "Value", "Text");
            TempData["Gender"] = genderList;

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

            return View(commissionModel.Item3);
        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.UpdateCommission, AllowInAccess = false)]
        public virtual ActionResult Update(string special, CommissionModel commissionModel)
        {
            if (!ModelState.IsValid)
                return Json(new { isError = true, Message = "ورودی نامعتبر!" });
            //var userId = long.Parse(User.Identity.GetUserId());
            //var data = _personManagementService.IsStudentExist(commissionModel.StudentNumber, userId,
            //            commissionModel.RequestType).Result;
            //if (!data.Item1)
            //    return Json(new { isError = true, Message = data.Item2 });

            string[] arrSpecialEducations = special.Split('&');
            if (arrSpecialEducations.Length > 1)
            {
                string[] fields;
                long specialId;

                for (var i = 0; i < arrSpecialEducations.Length - 1; i++)
                {
                    fields = arrSpecialEducations[i].Split(',');
                    specialId = Convert.ToInt64(fields[0]);
                    CommissionSpecialEducation commissionSpecialEducation = new CommissionSpecialEducation
                    {
                        // CommissionId = Convert.ToInt64(commissionModel.Id),
                        SpecialEducationId = specialId
                    };
                    commissionModel.CommissionSpecialEducations.Add(commissionSpecialEducation);
                }
            }
            AuthenticationType userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
            long levelId = Convert.ToInt64(User.LevelId());
            var result = _comissionService.UpdateCommissionRequest(commissionModel);
            return Json(new { isError = !result.Item1, Message = result.Item2 });
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.SendCommissionToCartable, AllowInAccess = true)]
        [HttpPost]
        public virtual ActionResult SendCommissionToCartable(long requestId, int rowNumber, string description, long fieldofStudyId)
        {
            //var userAuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), User.AuthenType(), true);
            //var levelId = Convert.ToInt64(User.LevelId());
            var signer = _baseInfoComissionCouncilService.WhereSigner(s => s.RowNumber == rowNumber && s.RequestType == RequestType.Comision).FirstOrDefault();
            var postId = signer?.PostId ?? 0;
            var userInSigners = _cartableService.GetPersonInSigners(postId, fieldofStudyId);
            if (userInSigners.Item3 == null)
                return Json(new { isError = true, Message = userInSigners.Item2 });
            var userIdFrom = Convert.ToInt64(User.Identity.GetUserId());
            var result = _cartableService.Confirmation(userInSigners.Item3.Id, userIdFrom, requestId, postId, rowNumber, description);
            return Json(new { isError = !result.Item1, Message = result.Item2 });
        }

        [HttpGet]
        [CustomAuthorize(RoleNameDefault = AccessLevel.SaveStudentProfile, AllowInAccess = true)]
        public virtual ActionResult GetStudentProfile(long studentNumber, int requestType)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var studentProfile = _personManagementService.GetStudentProfile(studentNumber, userId);

            var enumGrade = EnumerationService.GetEnumValues<Grade>();
            var enumGradeList = new SelectList(enumGrade, "Value", "Text");
            TempData["Grade"] = enumGradeList;

            var enumMilitaryServiceStatus = EnumerationService.GetEnumValues<MilitaryServiceStatus>();
            var enumMilitaryServiceStatusList = new SelectList(enumMilitaryServiceStatus, "Value", "Text");
            TempData["MilitaryServiceStatus"] = enumMilitaryServiceStatusList;

            var gender = EnumerationService.GetEnumValues<Gender>();
            var genderList = new SelectList(gender, "Value", "Text");
            TempData["Gender"] = genderList;

            var query = _structureManageService.GetAllFieldofStudy(userId, 1).Item3.AsEnumerable();
            if (query != null)
            {
                var fieldofStudies = (from f in query
                                      select new DropDownModel { Value = f.Id.ToString(), Text = f.OrganizationStructureName.Name }).ToList();
                var listFieldofStudy = new SelectList(fieldofStudies, "Value", "Text");
                TempData["FieldofStudy"] = listFieldofStudy;
            }
            else
                TempData["FieldofStudy"] = new List<DropDownModel>();
            return PartialView(MVC.Personel.Commission.Views._StudentProfile, studentProfile.Item3);

        }

        [HttpPost]
        [CustomAuthorize(RoleNameDefault = AccessLevel.SaveStudentProfile, AllowInAccess = false)]
        public virtual ActionResult SaveStudentProfile(StudentProfileModel studentProfileModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { isError = true, Message = @"ورودی نامعتبر!" });
                }
                if (studentProfileModel.Gender == Gender.Famele)
                    studentProfileModel.MilitaryServiceStatus = MilitaryServiceStatus.None;
                var levelId = Convert.ToInt64(User.LevelId());
                var personId = _personManagementService.GetPersonIdbyStudentNumber(studentProfileModel.StudentNumber);
                studentProfileModel.PersonId = personId;
                var data = _personManagementService.AddOrUpdateStudentProfile(studentProfileModel, levelId);
                return Json(new { isError = !data.Item1, Message = data.Item2 });
            }
            catch (Exception exception)
            {
                return Json(new { isError = true, Message = @"خطا در ثبت اطلاعات" });
            }
        }

        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCommissionShow, AllowInAccess = true)]
        public virtual ActionResult Attachment(long requestId)
        {
            ViewBag.RequestId = requestId;
            ViewBag.ControllerName = ControllerName.Commission;
            return PartialView(MVC.Personel.Cartable.Views._Attachment);
        }
        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCommissionShow, AllowInAccess = false)]
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

        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCommissionUpload, AllowInAccess = true)]
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

        [CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCommissionDelete, AllowInAccess = true)]
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


        //[CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCommission, AllowInAccess = true)]
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
        //        _unitOfWork.SaveChanges();
        //    }
        //    catch (Exception exception)
        //    {
        //        Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
        //        isSavedSuccessfully = false;
        //    }


        //    if (isSavedSuccessfully)
        //    {
        //        return Json(new { Message = fName });
        //    }
        //    return Json(new { Message = "Error in saving file" });
        //}

        //[CustomAuthorize(RoleNameDefault = AccessLevel.AttachmentCommission, AllowInAccess = false)]
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