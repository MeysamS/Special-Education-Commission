using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class UserRepository : MainRepository<User>, IUserRepository
    {
        readonly IDbSet<User> _user;
        readonly IUnitOfWork _unitOfWork;
        public UserRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _user = _unitOfWork.Set<User>();
        }
    }
    
}