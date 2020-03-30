using Comision.Model.Domain;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    public class RequestModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long PersonId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string NationalCode { get; set; }
        public Gender Gender { get; set; }
        public string GenderString { get; set; }
        public long StudentNumber { get; set; }
        public long FieldofStudyId { get; set; }
        public string FieldofStudyString { get; set; }
        public virtual FieldofStudy FieldofStudy { get; set; }
        public Grade Grade { get; set; }
        public string GradeString { get; set; }

        /// <summary>
        /// تعداد دفعات استفاده از رای شورای آموزشی
        /// </summary>
        public int NumberofVotesCouncil { get; set; }
        public RequestType RequestType { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public short NumberofSpentUnits { get; set; }
        public short NumberofRemainingUnits { get; set; }
        public MilitaryServiceStatus MilitaryServiceStatus { get; set; }
        public string MilitaryServiceStatusString { get; set; }
    }
}
