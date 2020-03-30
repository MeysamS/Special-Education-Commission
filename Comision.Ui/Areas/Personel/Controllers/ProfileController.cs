using System;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Enum;
using Comision.Service.ImplementationService;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Utility;
using Microsoft.AspNet.Identity;

namespace Comision.Ui.Areas.Personel.Controllers
{
    [AuthorizeUser]
    public partial class ProfileController : Controller
    {
        private readonly IPersonManagementService _personManagementService;
        private readonly IStructureManageService _structureManageService;
        public ProfileController(IPersonManagementService personManagementService, IStructureManageService iStructureManageService)
        {
            _personManagementService = personManagementService;
            _structureManageService = iStructureManageService;
        }

        // GET: Personel/Profile
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)] // will be applied to all actions in MyController, unless those actions override with their own decoration
        public virtual ActionResult Index()
        {
            try
            {
                var personId = long.Parse(User.Identity.GetUserId());
                var state = _personManagementService.GetProfileFullInfo(Convert.ToInt64(personId));
                //string s=~/ Content / Images / Avatars / @Model.ModelProfile.Avatar;
                if (!state.Item1) return View(MVC.Errors.Views.Error);
                var grade = EnumerationService.GetEnumValues<Grade>();
                var gradeList = new SelectList(grade, "Value", "Text");
                TempData["Grade"] = gradeList;

                var gender = EnumerationService.GetEnumValues<Gender>();
                var genderList = new SelectList(gender, "Value", "Text");
                TempData["Gender"] = genderList;

                var militaryServiceStatus = EnumerationService.GetEnumValues<MilitaryServiceStatus>();
                var militaryServiceStatusList = new SelectList(militaryServiceStatus, "Value", "Text");
                TempData["MilitaryServiceStatus"] = militaryServiceStatusList;

                var fieldofStudies = (from f in _structureManageService.GetAllFieldofStudy().Item3.AsEnumerable()
                                      select new DropDownModel { Value = f.Id.ToString(), Text = f.OrganizationStructureName.Name }).ToList();
                var listFieldofStudy = new SelectList(fieldofStudies, "Value", "Text");
                TempData["FieldofStudy"] = listFieldofStudy;
                ViewBag.Cancelurl = Url.Action(MVC.Personel.Profile.Index());
                ViewBag.ActionUrlSavePassword = Url.Action(MVC.Personel.Profile.SavePassword(), "http");
                return View(state.Item3);
            }
            catch (Exception)
            {
                return View(MVC.Errors.Views.Error);
            }
        }

        [HttpPost]
        public virtual ActionResult SaveProfile(ProfileModel profileModel, HttpPostedFileBase avatarFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { isError = true, Message = @"ورودی نامعتبر!" });
                }
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var addressUrlFile = new AddressUrlFile(Server.MapPath("~//"));
                    var path = addressUrlFile.Image;
                    // ذخیره تصویر شخصی
                    if (avatarFile != null)
                    {
                        if (Useful.CreateFolderIfNeeded(addressUrlFile.Image))
                        {
                            // اگر قبلا فایل وجود دارد حذف شود
                            if (System.IO.File.Exists(path + profileModel.Avatar))
                                System.IO.File.Delete(path + profileModel.Avatar);

                            if (profileModel.Avatar == null)
                                path += Guid.NewGuid() + Path.GetExtension(avatarFile.FileName);
                            else
                                path += profileModel.Avatar;

                            avatarFile.SaveAs(path);
                            profileModel.Avatar = Path.GetFileName(path);
                        }
                    }
                    var levelId = Convert.ToInt64(User.LevelId());
                    var data = _personManagementService.AddOrUpdateProfile(profileModel,levelId);
                    scope.Complete();
                    return Json(new { isError = !data.Item1, Message = data.Item2 });
                }
            }
            catch (Exception)
            {
                return Json(new { isError = true, Message = @"خطا در ویرایش اطلاعات پرسنلی" });
            }
        }

        [HttpPost]
        public virtual ActionResult SavePassword(PasswordModel passwordModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { isError = true, Message = @"ورودی نامعتبر!" });
                }
                var data = _personManagementService.AddOrUpdatePassword(passwordModel);
                return Json(new { isError = !data.Item1, Message = data.Item2 });
            }
            catch (Exception exception)
            {
                return Json(new { isError = true, Message = @"خطا در ویرایش تغییر کلمه عبور" });
            }
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult DeleteAvatar(long profileId)
        {
            try
            {
                var profile = _personManagementService.FindProfile(profileId);
                if (profile == null)
                    return Json(new { isError = true, Message = @"ورودی نامعتبر!" });

                profile.Avatar = "";
                var result = _personManagementService.UpdateProfile(profile);
                if (result.Item1 == false)
                {
                    var path = Server.MapPath("~\\Content\\Images\\Avatars\\" + profile.Avatar);
                    System.IO.File.Delete(path);
                }

                return Json(new { isError = !result.Item1, Message = result.Item2 });
            }
            catch (Exception)
            {
                return Json(new { isError = true, Message = @"خطا در ویرایش اطلاعات پرسنلی" });
            }

        }
    }
}