using System.ComponentModel.DataAnnotations;

namespace Comision.Ui.Models
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "آدرس ایمیل وارد نشده است!")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل معتبر نیست!")]
        [Display(Name = "آدرس ایمیل")]
        public string Email { get; set; }
    }
}