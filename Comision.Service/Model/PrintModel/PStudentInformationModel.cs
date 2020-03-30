using Comision.Model.Enum;

namespace Comision.Service.Model.PrintModel
{
    public class PStudentInformationModel
    {
        public long StudentId { get; set; }
        public string NameFamili { get; set; }
        public long StudentNumber { get; set; }
        public string FieldofStudyName { get; set; }

        public Gender Gender { get; set; }
        public Grade  Grade { get; set; }
        public string GradeName { get; set; }

        public int NumberofRemainingUnits { get; set; }
        public int NumberofSpentUnits { get; set; }

        public MilitaryServiceStatus MilitaryServiceStatus { get; set; }
        public string MilitaryServiceStatusName { get; set; }
        public long CountuseCouncil { get; set; }
        

    }
}
