using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Comision.Ui.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [Display(Name = @"ادرس ایمیل")]
        [Remote("IsEmailConfirmed", "Account", HttpMethod = "Post", ErrorMessage = @"حساب کاربری تائید نشده است، به ایمیل خود مراجعه نمائید")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }


        [Remote("CompareCaptcha", "Account", HttpMethod = "Post", ErrorMessage = @"کد امنیتی اشتباه است")]
        [Required(ErrorMessage = @"کد امنیتی وارد نشده است")]
        [Display(Name = "تصویر امنیتی")]
        public string Captcha { get; set; }
    }
}