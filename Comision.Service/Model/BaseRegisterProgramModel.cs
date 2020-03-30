using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    /// <summary>
    /// کلاس های مربوط به ثبت نام برنامه از آن ارث می برند ، به خاطر داشتن فیلد های مشترک
    /// </summary>
    public class BaseRegisterProgramModel
    {
        /// <summary>
        /// کد سازمان
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// نام سازمان
        /// </summary>
        public string OrganName { get; set; }
        /// <summary>
        /// تلفن
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [Display(Name = "ادرس ایمیل")]
        [Remote("IsEmailExist", "Account", HttpMethod = "Post", ErrorMessage = "ایمیل مورد نظر تکراری می باشد")]
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "فرمت ایمیل نامعتبر می باشد")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmedPassword")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "PasswordInvalid")]
        public string ConfirmPassword { get; set; }
        public RoleType RoleType { get; set; }

        //[Remote("HasValidIdentificationCode", "Account", HttpMethod = "Post", ErrorMessage = "کد شناسایی نا معتبر می باشد")]
        [Display(Name = "کد شناسایی")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string LicenceCode { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Family { get; set; }

        //[Remote("HasValidNationalCode", "Account", HttpMethod = "Post", ErrorMessage = "کد ملی نا معتبر می باشد")]
        [Display(Name = "کد ملی")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string NationalCode { get; set; }
        public string Mobile { get; set; }
    }
}
