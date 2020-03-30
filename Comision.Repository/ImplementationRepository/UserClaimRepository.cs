using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class UserClaimRepository : MainRepository<UserClaim>, IUserClaimRepository
    {
        readonly IDbSet<UserClaim> _userClaim;
        readonly IUnitOfWork _unitOfWork;
        public UserClaimRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userClaim = _unitOfWork.Set<UserClaim>();
        }
    }
}
