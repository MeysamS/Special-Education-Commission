using System;
using System.Linq;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Service.Enum;

namespace Comision.Service.IService
{
    public interface IMemberManagementService
    {
        Tuple<bool,string> AddOrUpdateMaster(MemberMaster member,StateOperation stateOperation);

        Tuple<bool, string> AddOrUpdateDetail(MemberDetails details,StateOperation stateOperation);

        IQueryable<MemberMaster> GetMasters(long univercityId,RequestType requestType);

        IQueryable<MemberDetails> GetDetail(long masterId);

        Tuple<bool, string> DeleteMaster(long id);

        Tuple<bool, string> DeleteDetail(long id);


    }
}
