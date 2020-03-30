using System;
using System.Collections.Generic;
using System.Linq;
using Comision.Model.Domain;
using Comision.Service.Model;

namespace Comision.Service.IService
{
    public interface IRequestService
    {
        Tuple<bool, string> AddorUpdateRequest(RequestModel requestModel);
        Tuple<bool, string, List<Request>> GetListRequest(long userId);
        Tuple<bool, string, DetailRequestModel> DetailRequest(long requestId, AddressUrlFile addressUrlFile);
        IQueryable<Request> GetRequestByStudentCode(long studentNumber);
        int GetRequestCount(long userId);
        Request Find(long requestId);
        Tuple<bool, string> Archive(long requestId,long userId, DateTime dateArchive);
        Tuple<bool, string> Delete(long requestId);
        Tuple<bool, string, List<RequestStudentModel>> ListRequestStudent(long studentNumber);
    }
}
