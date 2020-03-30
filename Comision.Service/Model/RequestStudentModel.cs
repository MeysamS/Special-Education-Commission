using System;
using Comision.Model.Domain;
using Comision.Model.Enum;

namespace Comision.Service.Model
{
    /// <summary>
    /// مدل درخواست برای دانشجو
    /// لیست تمام درخواست های دانشجو نشان داده می شود
    /// </summary>
    public class RequestStudentModel
    {
        public long Id { get; set; }
        public long CommissionCouncilNumber { get; set; }
        public string Description { get; set; }
        public DateTime DateRequest { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string NationalCode { get; set; }
        public Gender Gender { get; set; }
        public long StudentNumber { get; set; }
        public virtual string FieldofStudyName { get; set; }
        public Grade Grade { get; set; }
        public RequestType RequestType { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public MilitaryServiceStatus MilitaryServiceStatus { get; set; }
        public Vote Vote{ get; set; }
    }
}
