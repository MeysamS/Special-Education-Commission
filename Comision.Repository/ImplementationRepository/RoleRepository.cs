using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class RoleRepository : MainRepository<Role>, IRoleRepository
    {

        readonly IDbSet<Role> _role;
        IUnitOfWork _unitOfWork;
        public RoleRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _role = _unitOfWork.Set<Role>();
        }
     
    }
}