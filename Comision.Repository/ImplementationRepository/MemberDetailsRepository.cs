using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class MemberDetailsRepository : MainRepository<MemberDetails>, IMemberDetailsRepository
    {
        readonly IDbSet<MemberDetails> _memberDetails;
        IUnitOfWork _unitOfWork;
        public MemberDetailsRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _memberDetails = _unitOfWork.Set<MemberDetails>();
        }
    }
}
