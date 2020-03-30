using System.Data.Entity;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Repository.IRepository;

namespace Comision.Repository.ImplementationRepository
{
    public class CouncilRepository : MainRepository<Council>, ICouncilRepository
    {
        readonly IDbSet<Council> _council;
        IUnitOfWork _unitOfWork;
        public CouncilRepository(IUnitOfWork unitOfWork)
           : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _council = _unitOfWork.Set<Council>();
        }
    }
}
