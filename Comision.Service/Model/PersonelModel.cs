using System;
using System.ComponentModel.DataAnnotations;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Utility;

namespace Comision.Service.Model
{
    public class PersonelModel
    {
        /// <summary>
        /// ایدی شخص
        /// کلید اصلی و خارجی
        /// </summary>
        public virtual long PersonId { get; set; }

        /// <summary>
        /// شماره کارمندی
        /// </summary>
        [Display(Name = "شماره کارمندی")]
        public virtual string EmployeeNumber { get; set; }

        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        [Display(Name = "تاریخ استخدام")]
        [UIHint("PersianDatePicker")]
        public DateTime? DateOfEmployeement { get; set; }

        /// <summary>
        /// آخرین مدرک تحصیلی
        /// </summary>
        [Display(Name = "آخرین مدرک تحصیلی")]
        public virtual Grade Grade { get; set; }

        /// <summary>
        /// امضای الکترونیکی
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// فعال/غیر فعال
        /// </summary>
        [Display(Name = "فعال")]
        public virtual bool Active { get; set; }
        public PersonelModel() { }
        public PersonelModel(Personel personel):this()
        {
            if (personel == null)
                return;
            PersonId = personel.PersonId;
            DateOfEmployeement =personel.DateOfEmployeement;
            EmployeeNumber = personel.EmployeeNumber;
            Grade = personel.Grade;
            Signature = personel.Signature;
            Active = personel.Active;
        }
    }
}
