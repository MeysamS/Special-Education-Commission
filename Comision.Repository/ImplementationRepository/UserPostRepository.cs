using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class UserPostRepository : MainRepository<UserPost>, IUserPostRepository
    {
        readonly IDbSet<UserPost> _userPost;
        readonly IUnitOfWork _unitOfWork;
        public UserPostRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userPost = _unitOfWork.Set<UserPost>();
        }
    }
}
