using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class BranchProvinceRepository : MainRepository<BranchProvince>, IBranchProvinceRepository
    {
        readonly IDbSet<BranchProvince> _branchProvince;
        IUnitOfWork _unitOfWork;
        public BranchProvinceRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _branchProvince = _unitOfWork.Set<BranchProvince>();
        }
    }
}
