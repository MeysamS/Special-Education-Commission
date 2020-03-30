using System;
using System.ComponentModel.DataAnnotations;
using Comision.Model.Domain.UserDomain;

namespace Comision.Service.Model
{
    public class UserModel
    {
        public virtual long PersonId { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "Required")]
        [EmailAddress]
        [Display(Name = "آدرس الکترونیکی")]
        //[Remote("IsEmailExist", "Account",areaName:null, HttpMethod = "Post", ErrorMessage = "ایمیل مورد نظر تکراری می باشد")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "فرمت ایمیل نامعتبر می باشد")]
        public virtual string Email { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resources.Resources), ErrorMessageResourceName = "Required")]
        [EmailAddress]
        [Display(Name = "نام کاربری")]
        //[Remote("IsEmailExist", "Account", areaName: null, HttpMethod = "Post", ErrorMessage = "ایمیل مورد نظر تکراری می باشد")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "فرمت ایمیل نامعتبر می باشد")]
        public virtual string UserName { get; set; }

        /// <summary>
        /// True if the email is confirmed, default is false
        /// 
        /// </summary>
        [Display(Name = "تایید شده")]
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// 
        /// </summary>
        [Display(Name = "تاریخ غیر فعال شدن")]
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        /// <summary>
        /// Is lockout enabled for this user
        /// 
        /// </summary>
        [Display(Name = "فعال")]
        public virtual bool Active { get; set; }

        public UserModel() { }
        public UserModel(User user)
        {
            if (user == null)
                return;
            Email = user.Email;
            EmailConfirmed = user.EmailConfirmed;
            Active = !user.LockoutEnabled;
            LockoutEndDateUtc = user.LockoutEndDateUtc;
            UserName = user.UserName;
        }
    }
}
