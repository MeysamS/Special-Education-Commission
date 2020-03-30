using System.Collections.Generic;

namespace Comision.Service.Model.PrintModel
{
 public class PCouncilModel
    {
        public virtual PRequestDataModel PRequestDataModel { get; set; }
        public virtual PSignersModel PSignersModel { get; set; }
        public virtual List<PSignersListModel> PSignersListModel { get; set; }
        public virtual ICollection<PMemberDetailsModel> PMemberDetailsModel { get; set; }
        public virtual PStudentInformationModel PStudentInformationModel { get; set; }

    }
}
