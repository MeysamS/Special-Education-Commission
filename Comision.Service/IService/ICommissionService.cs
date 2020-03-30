using System;
using System.Collections.Generic;
using Comision.Model.Domain;
using Comision.Service.Model;
using Comision.Service.Enum;

namespace Comision.Service.IService
{
    public interface ICommissionService
    {
        Tuple<bool, string, long> AddCommissionRequest(CommissionModel commissionModel, long universityId);
        Tuple<bool, string, long> UpdateCommissionRequest(CommissionModel requestModel);
        Tuple<bool, string, CommissionModel> FindInfoCommission(long commissionId);
        //Tuple<bool, string> SendCommissiontoCartable(long commissionId, long userId, long universityId);
        Tuple<long, string> GetCommissionNumber(long universityId);
        Tuple<bool, string, List<Commission>> GetCommissionsProfileStudent(string searcvalue, SearchType searchtype);
    }
}
