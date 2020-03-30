using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Comision.Model.Domain;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class CommissionModel
    {
        public CommissionModel()
        {
            //SpecialEducations = new List<SpecialEducation>();
            CommissionSpecialEducations = new List<CommissionSpecialEducation>();
        }

        public long Id { get; set; }

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
        public string Description { get; set; }

        public long PersonId { get; set; }
        public string NameFamily { get; set; }
        public string NationalCode { get; set; }

        [Display(Name = @"جنسیت")]
        public Gender Gender { get; set; }
        public string GenderString { get; set; }
        public long StudentNumber { get; set; }
        public long FieldofStudyId { get; set; }
        public string FieldofStudy { get; set; }
        public Grade Grade { get; set; }
        public string GradeString { get; set; }
        public RequestType RequestType { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public short NumberofSpentUnits { get; set; }
        public short NumberofRemainingUnits { get; set; }
        public MilitaryServiceStatus MilitaryServiceStatus { get; set; }
        public string MilitaryServiceStatusString { get; set; }

        //public List<SpecialEducation> SpecialEducations { get; set; }
        public List<CommissionSpecialEducation> CommissionSpecialEducations { get; set; }
    }
}
