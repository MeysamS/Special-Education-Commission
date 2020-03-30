using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{

    public class UserRoleRepository : MainRepository<UserRole>, IUserRoleRepository
    {

        readonly IDbSet<UserRole> _userRole;
        IUnitOfWork _unitOfWork;
        public UserRoleRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRole = _unitOfWork.Set<UserRole>();
        }

    }
}
