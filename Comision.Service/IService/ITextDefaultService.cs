using System;
using System.Linq;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Service.Enum;

namespace Comision.Service.IService
{
    public interface ITextDefaultService
    {
        Tuple<bool, string> AddOrUpdate(TextDefault textDefault, StateOperation stateOperation);

        IQueryable<TextDefault> GetByType(long univercityId,TextDefaultType type);
        IQueryable<TextDefault> GetVoteCommissionText();
        IQueryable<TextDefault> GetVoteCouncilText();
        IQueryable<TextDefault> GetProblemText();
        IQueryable<TextDefault> GetSpecialEducationVerdictText();
        Tuple<bool, string> Delete(long id);
    }
}
