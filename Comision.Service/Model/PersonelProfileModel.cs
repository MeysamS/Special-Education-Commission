using System;
using System.ComponentModel.DataAnnotations;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    /// <summary>
    /// این کلاس الاعات پرسنل و اطلاعات شخصی را هم دارد
    /// </summary>
    public class PersonelProfileModel
    {
        /// <summary>
        /// ایدی شخص
        /// کلید اصلی و خارجی
        /// </summary>
        public long  PersonId { get; set; } 
        /// <summary>
        /// شماره کارمندی
        /// </summary>
        [Required(ErrorMessage = @"وارد نمودن این فیلد اجباریست")]
        [Display(Name = @"شماره پرسنلی")]
        public string EmployeeNumber { get; set; }
        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        [Display(Name = @"تاریخ استخدام")]
        [UIHint("PersianDatePicker")]
        public DateTime? DateOfEmployeement { get; set; }
        /// <summary>
        /// آخرین مدرک تحصیلی
        /// </summary>
        [Required(ErrorMessage = @"وارد نمودن این فیلد اجباریست")]
        [Display(Name = @"آخرین مدرک تحصیلی")]
        public Grade Grade { get; set; }

        // اطلاعات پروفایل        
        public virtual ProfileModel ModelProfile { get; set; }

       //public PersonelProfileModel ()
       // {
       //     ModelProfile = new ProfileModel();
       // }

    }
}
