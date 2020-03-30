using System;
using System.ComponentModel.DataAnnotations;
using Comision.Model.Domain;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class CouncilModel
    {
        public long Id { get; set; }

        /// <summary>
        /// شماره شورای آموزشی
        /// </summary>
        [Display(Name = @"شماره")]
        public long CouncilNumber { get; set; }

        /// <summary>
        /// تاریخ شورای آموزشی
        /// </summary>
        [UIHint("PersianDatePicker")]
        [Display(Name = @"تاریخ")]
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ProblemsCouncil{ get; set; }

        public long PersonId { get; set; }
        public string NameFamily { get; set; }
        public string NationalCode { get; set; }
        public Gender Gender { get; set; }
        public string GenderString { get; set; }
        public long StudentNumber { get; set; }
        public long FieldofStudyId { get; set; }
        public string FieldofStudy { get; set; }
        public Grade Grade { get; set; }
        public string GradeString { get; set; }

        /// <summary>
        /// تعداد دفعات استفاده از رای شورای آموزشی
        /// </summary>
        public int NumberofVotesCouncil { get; set; }
        public RequestType RequestType { get; set; }
        public RequestStatus RequestStatus{ get; set; }
        public short NumberofSpentUnits { get; set; }
        public short NumberofRemainingUnits { get; set; }
        public MilitaryServiceStatus MilitaryServiceStatus { get; set; }
        public string MilitaryServiceStatusString { get; set; }
    }
}
