using System;
using System.Configuration;
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Comision.Helper.Filters;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Service.ImplementationService;
using Comision.Service.IService;
using Comision.Utility;

namespace Comision.Ui.Areas.Admin.Controllers
{
    /// <summary>
    /// /
    /// </summary>
    [Authorize(Roles = "AdminUniversity")]
    public partial class SettingsController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }
        public virtual ActionResult Index()
        {
            var settings = _settingService.GetSettings();

            var ordinal = EnumerationService.GetEnumValues<Ordinal>();
            var genderList = new SelectList(ordinal, "Value", "Text");
            TempData["Ordinal"] = genderList;

            return View(settings);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddorUpdate(Settings settings, HttpPostedFileBase fileLogo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "ورودی نامعتبر");
                    return View(MVC.Admin.Settings.Views.Index, settings);
                }
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var path = Server.MapPath("~\\Content\\Images\\Logo\\");
                    // ذخیره لوگو
                    if (fileLogo != null)
                    {
                        if (Useful.CreateFolderIfNeeded(Server.MapPath("~/Content/Images/Logo")))
                        {
                            // اگر قبلا فایل وجود دارد حذف شود
                            if (System.IO.File.Exists(path + settings.University.Logo))
                                System.IO.File.Delete(path + settings.University.Logo);

                            if (settings.University.Logo == null)
                                path += Guid.NewGuid() + Path.GetExtension(fileLogo.FileName);
                            else
                                path += settings.University.Logo;

                            fileLogo.SaveAs(path);
                            settings.University.Logo = Path.GetFileName(path);
                        }
                    }

                    UpgradeWebconfig(settings.SmtpFrom, settings.SmtpHost, settings.SmtpUserName, settings.SmtpPass, Convert.ToInt32(settings.SmtpPort));
                    var data = _settingService.AddOrUpdateSettings(settings);
                    scope.Complete();

                    var ordinal = EnumerationService.GetEnumValues<Ordinal>();
                    var genderList = new SelectList(ordinal, "Value", "Text");
                    TempData["Ordinal"] = genderList;

                    return RedirectToAction(MVC.Admin.Settings.Index());
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", " خطا در ثبت تنظیمات عمومی");
                return View(MVC.Admin.Settings.Views.Index, settings);
            }
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult DeleteLogo(int settingId)
        {
            try
            {
                var setting = _settingService.Find(settingId);
                if (setting == null)
                    return Json(new { isError = true, Message = @"ورودی نامعتبر!" });

                setting.University.Logo = "";
                var result = _settingService.AddOrUpdateSettings(setting);
                if (result.Item1 == false)
                {
                    var path = Server.MapPath("~\\Content\\Images\\Logo\\" + setting.University.Logo);
                    System.IO.File.Delete(path);
                }

                return Json(new { isError = !result.Item1, Message = result.Item2 });
            }
            catch (Exception)
            {
                return Json(new { isError = true, Message = @"خطا در ویرایش اطلاعات پرسنلی" });
            }

        }
        public void UpgradeWebconfig(string from, string host, string username, string pass, int port)
        {
            var myConfig = WebConfigurationManager.OpenWebConfiguration("~");

            MailSettingsSectionGroup mailSection = myConfig.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            if (mailSection != null)
            {
                mailSection.Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailSection.Smtp.From = @from;
                mailSection.Smtp.Network.EnableSsl = true;
                mailSection.Smtp.Network.Host = host;
                mailSection.Smtp.Network.UserName = username;
                mailSection.Smtp.Network.Password = pass;
                mailSection.Smtp.Network.Port = port == 0 ? 1 : port;
            }
            myConfig.Save(ConfigurationSaveMode.Modified);

        }
    }
}