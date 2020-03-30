using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class PermissionRepository : MainRepository<Permission>, IPermissionRepository
    {
        readonly IDbSet<Permission> _permission;
        IUnitOfWork _unitOfWork;
        public PermissionRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _permission = _unitOfWork.Set<Permission>();
        }
    }
}
