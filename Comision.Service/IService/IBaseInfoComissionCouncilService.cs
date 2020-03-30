using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Service.Enum;
using Comision.Service.Model;

namespace Comision.Service.IService
{
    public interface IBaseInfoComissionCouncilService
    {
        Tuple<bool, string> AddOrUpdateSpecialEducation(SpecialEducationModel specialEducationModel, StateOperation stateOperation);
        Tuple<bool, string> AddOrUpdateSigner(SignerModel signerModel, StateOperation stateOperation);
        List<SpecialEducation> GetAllSpecialEducation();
        List<SpecialEducation> GetAllSpecialEducationforRequest(long requestId);
        List<Signer> GetAllSigner(RequestType requestType);
        List<Signer> WhereSigner(Expression<Func<Signer, bool>> predicate);
        Tuple<bool, string> DeleteSpecialEducation(long id);
        Tuple<bool, string> DeleteSigner(long id);
    }
}
