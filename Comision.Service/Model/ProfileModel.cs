using System;
using System.ComponentModel.DataAnnotations;
using Comision.Model.Domain.UserDomain;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    /// <summary>
    /// مدل پروفایل 
    /// </summary>
    public class ProfileModel
    {
        // اطلاعات پروفایل
        public virtual long PersonId { get; set; }

        [Required(ErrorMessage = @"وارد نمودن این فیلد اجباریست")]
        [Display(Name = @"نام")]
        public virtual string Name { get; set; }
        [Required(ErrorMessage = @"وارد نمودن این فیلد اجباریست")]
        [Display(Name = @"نام خانوادگی")]
        public virtual string Family { get; set; }

        [Display(Name = @"موبایل")]
        public virtual string Mobile { get; set; }

        [UIHint("PersianDatePicker")]
        [Display(Name = @"تاریخ تولد")]
        public virtual DateTime? BrithDate { get; set; }
        //public DateTime? BrithDate
        //{
        //    get { return BrithDate; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            System.Globalization.PersianCalendar x = new System.Globalization.PersianCalendar();
        //            BrithDate = x.ToDateTime(value.Value.Year, value.Value.Month, value.Value.Day, 0, 0, 0, 0, 0);
        //        }
        //        else
        //            BrithDate = value;
        //    }
        //}
        [Display(Name = @"جنسیت")]
        public virtual Gender Gender { get; set; }
        [Display(Name = @"تصویر پروفایل")]
        public virtual string Avatar { get; set; }
        [Display(Name = @"ایمیل")]
        public virtual string EmailUnivercity { get; set; }
        [Display(Name = @"آردس")]
        public virtual string AddressWork { get; set; }
        [Display(Name = @"تلفن")]
        public virtual string PhoneWork { get; set; }
        [Display(Name = "ادرس سایت")]
        public virtual string WebSite { get; set; }

        [Display(Name = @"توضیحات")]
        public virtual string Description { get; set; }
        [Display(Name = @"کد ملی")]
        public virtual string NationalCode { get; set; }

        [Display(Name = @"شماره شناسنامه")]
        public virtual string BirthcertificateNumber { get; set; }

        public virtual string NameFamili => Name + " " + Family;

        public ProfileModel()
        { }
        /// <summary>
        /// سازنده یک پروفایل دریافت و در مدل پروفایل کپی می کند
        /// </summary>
        /// <param name="profile"></param>
        public ProfileModel(Profile profile)
        {
            PersonId = profile.PersonId;
            Name = profile.Name;
            Family = profile.Family;
            Mobile = profile.Mobile;
            if(profile.BrithDate !=  null)
                BrithDate = profile.BrithDate;

            Gender = profile.Gender;
            Avatar = profile.Avatar;
            EmailUnivercity = profile.EmailUnivercity;
            AddressWork = profile.AddressWork;
            PhoneWork = profile.PhoneWork;
            WebSite = profile.WebSite;
            Description = profile.Description;
            NationalCode = profile.NationalCode;
            BirthcertificateNumber = profile.BirthcertificateNumber;
        }
    }
}
