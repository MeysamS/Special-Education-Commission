using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class MemberMasterRepository : MainRepository<MemberMaster>, IMemberMasterRepository
    {
        readonly IDbSet<MemberMaster> _memberMaster;
        readonly IUnitOfWork _unitOfWork;
        public MemberMasterRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _memberMaster = _unitOfWork.Set<MemberMaster>();
        }
    }
}
