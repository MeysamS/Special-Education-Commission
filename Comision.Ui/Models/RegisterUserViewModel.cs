using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Comision.Ui.Models
{
    public class RegisterUserViewModel
    {

        //[Remote("HasValidIdentificationCode", "Account", HttpMethod = "Post", ErrorMessage = "کد شناسایی نا معتبر می باشد")]
        [Display(Name = "کد شناسایی")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string IdentificationCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("IsEmailExist", "Account", HttpMethod = "Post", ErrorMessage = "ایمیل مورد نظر تکراری می باشد")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "فرمت ایمیل نامعتبر می باشد")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessage = "حداقل طول رشته 6 کاراکتر می باشد", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = @"Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = @"ConfirmedPassword")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "کلمه عبور مطابقت ندارد!")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string ConfirmPassword { get; set; }

        //[Required(ErrorMessage = "وارد نمودن این فیلد الزامیست")]
        //[Remote("CompareCaptcha", "Account", HttpMethod = "Post", ErrorMessage = "CaptchaInvalid")]
        //[Display(Name = "Captcha")]
        public string Captcha { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Family { get; set; }

        [Remote("HasValidNationalCode", "Account", HttpMethod = "Post", ErrorMessage = "کد ملی نا معتبر می باشد یا در سیستم موجود می باشد!")]
        [Display(Name = @"کد ملی")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
    }
}
