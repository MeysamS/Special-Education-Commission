using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using System.Web.UI;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.IService;
using Comision.Service.Model;
using Comision.Ui.Models;
using Microsoft.AspNet.Identity;
using Postal;

namespace Comision.Ui.Controllers
{
    [Authorize]
    public partial class AccountController : Controller
    {
        private readonly IAuthenticationManagementService _authenticationManagementService;
        private readonly IPersonManagementService _personManagementService;
        private readonly IUniversityRepository _universityRepository;
        private readonly ISliderService _sliderService;

        public AccountController(IAuthenticationManagementService authenticationManagementService,
            IPersonManagementService personManagementService, IUniversityRepository universityRepository,
            ISliderService sliderService)
        {
            _authenticationManagementService = authenticationManagementService;
            _personManagementService = personManagementService;
            _universityRepository = universityRepository;
            _sliderService = sliderService;
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult RegisterProgramCentralOrganization()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult RegisterProgramCentralOrganization(RegisterProgramCentralOrganizationModel registerProgramCentralOrganizationModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerProgramCentralOrganizationModel);
            }
            if (_authenticationManagementService.RegisterProgramCentralOrganization(registerProgramCentralOrganizationModel))
                return RedirectToAction(MVC.Account.Login(""));
            ModelState.AddModelError("InvalidRegister", @"شما قادر به ثبت نام برنام نمی باشید!");
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult RegisterProgramBranchProvince()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual ActionResult RegisterProgramBranchProvince(RegisterProgramBranchProvinceModel registerProgramBranchProvinceModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerProgramBranchProvinceModel);
            }
            if (_authenticationManagementService.RegisterProgramBranchProvince(registerProgramBranchProvinceModel))
                return RedirectToAction(MVC.Account.Login(""));
            ModelState.AddModelError("InvalidRegister", "شما قادر به ثبت نام برنامه نمی باشید!");
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult RegisterProgramUniversity()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public virtual ActionResult RegisterProgramUniversity(RegisterProgramUniversityModel registerProgramUniversityModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerProgramUniversityModel);
            }
            if (_authenticationManagementService.RegisterProgramUniversity(registerProgramUniversityModel))
                return RedirectToAction(MVC.Account.Login(""));
            ModelState.AddModelError("InvalidRegister", "شما قادر به ثبت نام دانشگاه نمی باشید!");
            return View();
        }

        [AllowAnonymous]
        public virtual ActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (!ModelState.IsValid)
                    {
                        //var errorList = ModelState.Values.SelectMany(m => m.Errors)
                        //                 .Select(e => e.ErrorMessage)
                        //                 .ToList();
                        ModelState.AddModelError("", @"ورودی نامعتبر!");
                        return View(registerUserViewModel);
                    }

                    var addUser =
                        await _authenticationManagementService.AddUser(registerUserViewModel.IdentificationCode,
                            registerUserViewModel.Email, registerUserViewModel.Password, registerUserViewModel.Name,
                            registerUserViewModel.Family, registerUserViewModel.NationalCode,
                            registerUserViewModel.Mobile);
                    if (addUser.Item1)
                    {
                        var callbackUrl = Url.Action(MVC.Account.ActionNames.ConfirmEmail, MVC.Account.Name,
                            new { userId = addUser.Item4.Id, code = addUser.Item3 }, Request.Url?.Scheme);
                        //await _userManager.SendEmailAsync(user.Id, "تائید حساب کاربری ","جهت فعال سازی حساب کاربری خود بر روی لینک زیر کلیک کنید: <a href=\"" + callbackUrl +"\">link</a>");
                        ViewBag.Link = callbackUrl;
                        var univers = _universityRepository.All().FirstOrDefault();
                        dynamic email = new Email("Reg.Html");
                        email.To = addUser.Item4.Email;
                        email.Title = "تائید حساب کاربری ";
                        email.Subject = (univers == null ? "تائید حساب کاربری " : "تائید حساب کاربری " + univers.Name);
                        email.Body = "جهت فعال سازی حساب کاربری خود بر روی لینک زیر کلیک کنید ";
                        email.Link = callbackUrl;
                        if (!string.IsNullOrWhiteSpace(email.To.ToString()))
                          //  email.Send();
                        //_unitOfWork.CommitTransaction();
                        scope.Complete();

                        return View(MVC.Account.Views.DisplayEmail);
                    }
                    ModelState.AddModelError("InvalidUser", addUser.Item2);
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("InvalidUser", @"شما قادر به ثبت نام نمی باشید!");
                return View();
            }
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public virtual async Task<ActionResult> ConfirmEmail(int? userId, string code)
        {
            if (userId == null || code == null)
            {
                return View(MVC.Errors.Views.Error);
            }
            var result = await _authenticationManagementService.ConfirmEmail(userId.Value, code);
            return View(result.Item1 ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            //var sliders = _sliderService.GetListSliderPath(new AddressUrlFile(Path.Combine("\\")));
            //ViewBag.ReturnUrl = returnUrl;
            //var str = "'" + sliders[0] + "'";
            //for (var i = 1; i < sliders.Count; i++)
            //{
            //    str += "," + "'" + sliders[i] + "'";
            //}
            //ViewBag.Sliders = sliders;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var login = await _authenticationManagementService.Login(model.Email, model.Password);
            if ((login.Item1 == false) || (login.Item2 == null))
            {
                ModelState.AddModelError("InvalidUser", "نام کاربری یا رمز عبور اشتباه است");
                return View(model);
            }
            if (await _authenticationManagementService.SignInAsync(login.Item2, model.RememberMe))
            {
                return RedirectToAction(MVC.Account.CheckProfile(login.Item2.Id));
            }
            ModelState.AddModelError("InvalidUser", "شما قادر به ورود نمی باشید!");
            return View(model);
        }

        [AllowAnonymous]
        public virtual ActionResult CheckProfile(long? userId)
        {
            var uid = userId ?? long.Parse(User.Identity.GetUserId());
            var checkProfile = _authenticationManagementService.CheckProfile(uid);
            if (!checkProfile.Item1) return RedirectToAction(MVC.User.Profile.SaveProfile());
            switch (checkProfile.Item2)
            {
                case AuthenticationType.AdminUniversity:
                    return RedirectToAction(MVC.Admin.Person.Index());
                case AuthenticationType.Personel:
                    var postSignersofPerson = _personManagementService.GetPostSignersofPersonel(uid);
                    if (postSignersofPerson.Item3.Any(p => p.Post.Signers.Count > 0))
                    {
                        return postSignersofPerson.Item3.Any(p => p.Post.Signers.Any(s => s.RowNumber == 1)) ?
                            RedirectToAction(MVC.Personel.Request.Index()) :
                            RedirectToAction(MVC.Personel.Cartable.Index());
                    }
                    break;
            }
            return HttpNotFound();
        }

        [AllowAnonymous]
        public virtual ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);
                var userBool = await _authenticationManagementService.IsEmailConfirmed(model.Email);
                if (userBool.Item2 == null)
                {
                    ModelState.AddModelError("", "آدرس الکترونیکی مورد نظر احراز هویت!");
                    return View(MVC.Account.Views.ForgetPassword);
                }
                var code = await _authenticationManagementService.GeneratePasswordAsync(userBool.Item2.Id);
                var callbackUrl = Url.Action(MVC.Account.ActionNames.ResetPassword, MVC.Account.Name,
                    new { userId = userBool.Item2.Id, code }, Request.Url.Scheme);
                var univers = _universityRepository.All().FirstOrDefault();
                //await
                //    _authenticationManagementService.SendEmailAsync(userBool.Item2.Id,
                //        "بازسازی کلمه عبور / " + univers?.Name,
                //        "لطفا کلمه عبور خود را بازسازی کنید: <a href=\"" + callbackUrl + "\">link</a>");
                
                dynamic email = new Email("Reg.Html");
                email.To = userBool.Item2.Email;
                email.Title = "بازسازی کلمه عبور ";
                email.Subject = "بازسازی کلمه عبور / " + univers?.Name;
                email.Body = "جهت بازسازی کلمه عبور خود بر روی لینک زیر کلیک کنید ";
                email.Link = callbackUrl;
                if (!string.IsNullOrWhiteSpace(email.To.ToString()))
                    email.Send();

                ViewBag.Link = callbackUrl;
                return View(MVC.Account.Views.ForgetPasswordConfirmation);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", "خطا در بازسازی کلمه عبور!");
                return View();
            }
        }

        [AllowAnonymous]
        public virtual ActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public virtual ActionResult ResetPassword(string code)
        {
            return code == null ? View(MVC.Errors.Views.Error) : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _authenticationManagementService.IsEmailExist(model.Email);
            if (user.Item2 == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(MVC.Account.ResetPasswordConfirmation());
            }
            var result = await _authenticationManagementService.ResetPasswordAsync(user.Item2.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(MVC.Account.ResetPasswordConfirmation());
            }
            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public virtual ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> IsEmailExist(string email)
        {
            var user = await _authenticationManagementService.IsEmailExist(email);
            return Json(!user.Item1);
        }

        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual ActionResult CompareCaptcha(string captcha)
        {
            return Json(captcha == (string)Session["captcha"]);
        }

        [HttpPost]
        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual async Task<ActionResult> IsEmailConfirmed(string email)
        {
            var user = await _authenticationManagementService.IsEmailConfirmed(email);
            return Json(user.Item1);

        }


        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public virtual async Task<ActionResult> HasValidIdentificationCode(string identificationCode)
        {
            return Json(await _authenticationManagementService.HasValidIdentificationCode(identificationCode));
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public virtual async Task<ActionResult> HasValidNationalCode(string nationalCode)
        {
            var exsistNationalCode = await _authenticationManagementService.HasValidNationalCode(nationalCode);
            return Json(!exsistNationalCode);
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public virtual ActionResult IsStudentExist(long studentNumber, int requestType)
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var result = _personManagementService.IsStudentExist(studentNumber, userId);
            return Json(new { IsError = !result.Item1, Message = result.Item2, Data = result.Item3, JsonRequestBehavior.AllowGet });
            //return Json(result.Item3);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public virtual async Task<ActionResult> LogOff()
        {
            var uId = int.Parse(User.Identity.GetUserId());
            var result = await _authenticationManagementService.Logout(uId);
            if (result == false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction(MVC.Account.Login());
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}