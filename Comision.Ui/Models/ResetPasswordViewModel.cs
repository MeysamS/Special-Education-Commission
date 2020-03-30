using System.ComponentModel.DataAnnotations;

namespace Comision.Ui.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "آدرس الکترونیکی")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "حداقل طول رشته 6 کاراکتر می باشد", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور")]
        [Compare("Password", ErrorMessage = "کلمه عبور مطابقت ندارد!")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}