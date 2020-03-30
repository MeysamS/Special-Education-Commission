using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain.UserDomain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class ProfileRepository : MainRepository<Profile>, IProfileRepository
    {
        readonly IDbSet<Profile> _profile;
        IUnitOfWork _unitOfWork;
        public ProfileRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _profile = _unitOfWork.Set<Profile>();
        }
    }
}
