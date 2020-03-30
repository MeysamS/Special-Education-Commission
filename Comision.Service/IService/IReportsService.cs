using Comision.Service.Enum;
using Comision.Service.Model;
using Comision.Service.Model.PrintModel;

namespace Comision.Service.IService
{
  public  interface  IReportsService
  {
      PCommissionModel PrintCommission(long commissionId, AddressUrlFile addressUrlFile);
      PCouncilModel PrintCouncil(long councilId, AddressUrlFile addressUrlFile);
      PCommissionCouncilAllModel PrintCommissionAll(string searcvalue, SearchType searchtype);
      PCommissionCouncilAllModel PrintCouncilAll(string searcvalue, SearchType searchtype);
      string GetLogoUrl(AddressUrlFile addressUrlfile);
  }
}
