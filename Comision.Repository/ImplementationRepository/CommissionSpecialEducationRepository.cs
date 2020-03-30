using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class CommissionSpecialEducationRepository : MainRepository<CommissionSpecialEducation>, ICommissionSpecialEducationRepository
    {
        readonly IDbSet<CommissionSpecialEducation> _commissionSpecialEducation;
        IUnitOfWork _unitOfWork;
        public CommissionSpecialEducationRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _commissionSpecialEducation = _unitOfWork.Set<CommissionSpecialEducation>();
        }
    }
}
