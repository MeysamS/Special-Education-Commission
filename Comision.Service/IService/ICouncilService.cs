using System;
using System.Collections.Generic;
using Comision.Model.Domain;
using Comision.Service.Enum;
using Comision.Service.Model;

namespace Comision.Service.IService
{
    public interface ICouncilService
    {
        Tuple<bool, string, long> AddCouncilRequest(CouncilModel requestModel, long universityId);
        Tuple<bool, string, long> UpdateCouncilRequest(CouncilModel requestModel);
        Tuple<bool, string, CouncilModel> FindInfoCouncil(long councilId);
        //Tuple<bool, string> SendCounciltoCartable(long councilId, long userId, long universityId);
        Tuple<long, string> GetCouncilNumber(long universityId);
        long GetNumberofVotesCouncil(long studentId);
        Tuple<bool, string, List<Council>> GetCouncilsProfileStudent(string searcvalue, SearchType searchtype);
    }
}
