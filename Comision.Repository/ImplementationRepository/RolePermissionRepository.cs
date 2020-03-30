using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class RolePermissionRepository : MainRepository<RolePermission>, IRolePermissionRepository
    {
        readonly IDbSet<RolePermission> _rolePermission;
        IUnitOfWork _unitOfWork;
        public RolePermissionRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _rolePermission = _unitOfWork.Set<RolePermission>();
        }
    }
}
