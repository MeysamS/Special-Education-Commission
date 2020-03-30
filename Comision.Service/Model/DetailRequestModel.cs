using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class DetailRequestModel
    {
        public DetailRequestModel()
        {
            DetailRequestSignerModels = new List<DetailRequestSignerModel>();
        }
        public long RequestId { get; set; }

        /// <summary>
        /// شماره کمیسیون
        /// </summary>
        [Display(Name = @"شماره")]
        public long CommissionNumber { get; set; }

        /// <summary>
        /// تاریخ کمیسیون
        /// </summary>
        [UIHint("PersianDatePicker")]
        [Display(Name = @"تاریخ")]
        public DateTime Date { get; set; }

        [Display(Name = @"توضیحات")]
        public string Description { get; set; }

        [Display(Name = @"نام")]
        public string Name { get; set; }

        [Display(Name = @"نام خانوادگی")]
        public string Family { get; set; }

        [Display(Name = @"کد ملی")]
        public string NationalCode { get; set; }

        [Display(Name = @"شماره دانشجویی")]
        public long StudentNumber { get; set; }

        public long? UniversityId { get; set; }
        public long? CollegeId { get; set; }
        public long? EducationGroupId { get; set; }
        public long FieldofStudyId { get; set; }

        [Display(Name = @"رشته تحصیلی")]
        public string FieldofStudy { get; set; }

        [Display(Name = @"تعدادواحد گذرانده")]
        public short NumberofSpentUnits { get; set; }

        [Display(Name = @"تعداد واحد باقیمانده")]
        public short NumberofRemainingUnits { get; set; }

        [Display(Name = @"مقطع تحصیلی")]
        public Grade Grade { get; set; }

        [Display(Name = @"نوع درخواست")]
        public RequestType RequestType { get; set; }

        [Display(Name = @"وضعیت درخواست")]
        public RequestStatus RequestStatus { get; set; }

        [Display(Name = @"مشکل آموزشی")]
        public string ProblemsCouncil { get; set; }

        [Display(Name = @"متن رای")]
        public string VoteText { get; set; }

        [Display(Name = @"وضعیت نظام وظیفه")]
        public MilitaryServiceStatus MilitaryServiceStatus { get; set; }
        public Gender Gender { get; set; }
       
        [Display(Name = @"امضا کنندگان")]
        public List<DetailRequestSignerModel> DetailRequestSignerModels { get; set; }

        // لیست موارد هایی که مقدار ترو دارند 
        public List<DetailSpecialEducationModel> DetailSpecialEducationModels { get; set; }
    }
}
