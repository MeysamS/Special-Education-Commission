using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class VoteRepository : MainRepository<Vote>, IVoteRepository
    {
        readonly IDbSet<Vote> _vote;
        IUnitOfWork _unitOfWork;
        public VoteRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _vote = _unitOfWork.Set<Vote>();
        }
    }
}
