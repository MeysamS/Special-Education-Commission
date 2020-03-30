using System.Collections.Generic;

namespace Comision.Service.Model.PrintModel
{
  public  class PCommissionCouncilAllModel
    {
        public virtual List<PRequestAllDataModel> PRequestAllDataModels { get; set; }

        public virtual List<PMemberDetailsModel> PMemberDetailsModel { get; set; }
    }
}
