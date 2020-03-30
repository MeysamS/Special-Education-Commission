using System;
using System.Collections.Generic;

namespace Comision.Service.Model.PrintModel
{
    public class PRequestAllDataModel
    {
        public long RequestId { get; set; }
        public long CommissionCouncilNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string VoteText { get; set; }
        public long StudentNumber { get; set; }
        public string NameFamili { get; set; }
        public virtual ICollection<PSpecialEducationModel> PSpecialEducationModel { get; set; }
    }
}
