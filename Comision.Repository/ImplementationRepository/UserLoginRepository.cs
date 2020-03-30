using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class UserLoginRepository : MainRepository<UserLogin>, IUserLoginRepository
    {
        readonly IDbSet<UserLogin> _userLogin;
        readonly IUnitOfWork _unitOfWork;
        public UserLoginRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userLogin = _unitOfWork.Set<UserLogin>();
        }
    }
}
