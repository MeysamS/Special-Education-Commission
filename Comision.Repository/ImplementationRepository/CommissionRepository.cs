using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class CommissionRepository : MainRepository<Commission>, ICommissionRepository
    {
        readonly IDbSet<Commission> _commission;
        IUnitOfWork _unitOfWork;
        public CommissionRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _commission = _unitOfWork.Set<Commission>();
        }
    }
}
