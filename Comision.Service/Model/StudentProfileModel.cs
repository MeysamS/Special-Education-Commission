using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class StudentProfileModel
    {
        public long PersonId { get; set; }

        [Required(ErrorMessage = @"وارد نمودن این فیلد اجباریست")]
        [Display(Name = @"شماره دانشجویی")]
        public long StudentNumber { get; set; }

        [Display(Name = "رشته تحصیلی")]
        public virtual long FieldofStudyId { get; set; }

        [Display(Name = "مقطع تحصیلی")]
        public virtual Grade Grade { get; set; }

        [Display(Name = "وضعیت خدمت سربازی")]
        public virtual MilitaryServiceStatus MilitaryServiceStatus { get; set; }

        [Required(ErrorMessage = @"وارد نمودن این فیلد اجباریست")]
        [Display(Name = @"نام")]
        public virtual string Name { get; set; }
        [Required(ErrorMessage = @"وارد نمودن این فیلد اجباریست")]
        [Display(Name = @"نام خانوادگی")]
        public virtual string Family { get; set; }

        [Display(Name = @"جنسیت")]
        public virtual Gender Gender { get; set; }

        [Display(Name = @"کد ملی")]
        public virtual string NationalCode { get; set; }
    }
}
